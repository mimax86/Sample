using FluentAssertions;
using Moq;
using NUnit.Framework;
using Transactions.Interfaces;
using Transactions.Logic.Netting;
using Transactions.Statics;

namespace Transaction.Tests.Netting
{
    [TestFixture]
    public class NettingWeightCalculatorTests
    {
        private INettingWeightCalculator _calculator;
        private INettingTarget _primaryBuy;
        private INettingTarget _primarySell;
        private INettingSource _secondaryBuy, _secondaryBuyDirectionChanging;
        private INettingSource _secondarySell, _secondarySellDirectionChanging;
        private INettingSource _adjustment;

        [SetUp]
        public void TestFixtureSetup()
        {
            _calculator = new NettingWeightCalculator();

            var unit = new Unit {Name = "ozt"};

            var primaryBuyMock = new Mock<INettingTarget>();
            primaryBuyMock.Setup(transfer => transfer.Type).Returns(TransferType.TransferBuyType1);
            primaryBuyMock.Setup(transfer => transfer.AccountingWeight).Returns(1000m);
            primaryBuyMock.Setup(transfer => transfer.ImbalanceWeight).Returns(-100m);
            primaryBuyMock.Setup(transfer => transfer.AdjustmentWeight).Returns(0m);
            primaryBuyMock.Setup(transfer => transfer.AccountingUnit).Returns(unit);
            _primaryBuy = primaryBuyMock.Object;

            var primarySellMock = new Mock<INettingTarget>();
            primarySellMock.Setup(transfer => transfer.Type).Returns(TransferType.TransferSellType1);
            primarySellMock.Setup(transfer => transfer.AccountingWeight).Returns(1000m);
            primarySellMock.Setup(transfer => transfer.ImbalanceWeight).Returns(-100m);
            primarySellMock.Setup(transfer => transfer.AdjustmentWeight).Returns(0m);
            primarySellMock.Setup(transfer => transfer.AccountingUnit).Returns(unit);
            _primarySell = primarySellMock.Object;

            var qpBuyMock = new Mock<INettingSource>();
            qpBuyMock.Setup(transfer => transfer.Type).Returns(TransferType.TransferBuyType1);
            qpBuyMock.Setup(transfer => transfer.AccountingWeight).Returns(100m);
            qpBuyMock.Setup(transfer => transfer.AdjustmentWeight).Returns(0m);
            _secondaryBuy = qpBuyMock.Object;

            var qpBuyDirectionChangingMock = new Mock<INettingSource>();
            qpBuyDirectionChangingMock.Setup(transfer => transfer.Type).Returns(TransferType.TransferBuyType1);
            qpBuyDirectionChangingMock.Setup(transfer => transfer.AccountingWeight).Returns(1500m);
            qpBuyDirectionChangingMock.Setup(transfer => transfer.AdjustmentWeight).Returns(0m);
            _secondaryBuyDirectionChanging = qpBuyDirectionChangingMock.Object;

            var qpSellMock = new Mock<INettingSource>();
            qpSellMock.Setup(transfer => transfer.Type).Returns(TransferType.TransferSellType1);
            qpSellMock.Setup(transfer => transfer.AccountingWeight).Returns(100m);
            qpSellMock.Setup(transfer => transfer.AdjustmentWeight).Returns(0m);
            _secondarySell = qpSellMock.Object;

            var qpSellDirectionChangingMock = new Mock<INettingSource>();
            qpSellDirectionChangingMock.Setup(transfer => transfer.Type).Returns(TransferType.TransferSellType1);
            qpSellDirectionChangingMock.Setup(transfer => transfer.AccountingWeight).Returns(1500m);
            qpSellDirectionChangingMock.Setup(transfer => transfer.AdjustmentWeight).Returns(0m);
            _secondarySellDirectionChanging = qpSellDirectionChangingMock.Object;

            var qpAdjustmentMock = new Mock<INettingSource>();
            qpAdjustmentMock.Setup(transfer => transfer.Type).Returns(TransferType.TransferAdjustmentType);
            qpAdjustmentMock.Setup(transfer => transfer.AccountingWeight).Returns(0m);
            qpAdjustmentMock.Setup(transfer => transfer.AdjustmentWeight).Returns(100m);
            _adjustment = qpAdjustmentMock.Object;
        }

        [Test]
        public void NettingBuyWithBuyTest()
        {
            var nettingResult = _calculator.GetNettingResult(_primaryBuy, new[] { _secondaryBuy });
            nettingResult.ResultWeight.ShouldBeEquivalentTo(_primaryBuy.AccountingWeight + _secondaryBuy.AccountingWeight);
            nettingResult.ResultImbalance.ShouldBeEquivalentTo(_primaryBuy.ImbalanceWeight - _secondaryBuy.AccountingWeight);
            nettingResult.DirectionChanging.Should().BeFalse();
        }

        [Test]
        public void NettingBuyWithSellTest()
        {
            var nettingResult = _calculator.GetNettingResult(_primaryBuy, new[] { _secondarySell });
            nettingResult.ResultWeight.ShouldBeEquivalentTo(_primaryBuy.AccountingWeight - _secondarySell.AccountingWeight);
            nettingResult.ResultImbalance.ShouldBeEquivalentTo(_primaryBuy.ImbalanceWeight + _secondarySell.AccountingWeight);
        }

        [Test]
        public void NettingBuyWithSellDirectionChangingTest()
        {
            var nettingResult = _calculator.GetNettingResult(_primaryBuy, new[] { _secondarySellDirectionChanging });
            nettingResult.ResultWeight.ShouldBeEquivalentTo(_primaryBuy.AccountingWeight - _secondarySellDirectionChanging.AccountingWeight);
            nettingResult.ResultImbalance.ShouldBeEquivalentTo(_primaryBuy.ImbalanceWeight + _secondarySellDirectionChanging.AccountingWeight);
            nettingResult.DirectionChanging.Should().BeTrue();
        }

        [Test]
        public void NettingSellWithSellTest()
        {
            var nettingResult = _calculator.GetNettingResult(_primarySell, new[] { _secondarySell });
            nettingResult.ResultWeight.ShouldBeEquivalentTo(-_primarySell.AccountingWeight - _secondarySell.AccountingWeight);
            nettingResult.ResultImbalance.ShouldBeEquivalentTo(_primarySell.ImbalanceWeight - _secondarySell.AccountingWeight);
        }

        [Test]
        public void NettingSellWithBuyTest()
        {
            var nettingResult = _calculator.GetNettingResult(_primarySell, new[] { _secondaryBuy });
            nettingResult.ResultWeight.ShouldBeEquivalentTo(-_primarySell.AccountingWeight + _secondaryBuy.AccountingWeight);
            nettingResult.ResultImbalance.ShouldBeEquivalentTo(_primarySell.ImbalanceWeight + _secondaryBuy.AccountingWeight);
        }

        [Test]
        public void NettingSellWithBuyDirectionChangingTest()
        {
            var nettingResult = _calculator.GetNettingResult(_primarySell, new[] { _secondaryBuyDirectionChanging });
            nettingResult.ResultWeight.ShouldBeEquivalentTo(-_primarySell.AccountingWeight + _secondaryBuyDirectionChanging.AccountingWeight);
            nettingResult.ResultImbalance.ShouldBeEquivalentTo(_primarySell.ImbalanceWeight + _secondaryBuyDirectionChanging.AccountingWeight);
            nettingResult.DirectionChanging.Should().BeTrue();
        }

        [Test]
        public void UnnettingBuyWithBuyTest()
        {
            var unnettingResult = _calculator.GetUnnettingResult(_primaryBuy, new[] { _secondaryBuy });
            unnettingResult.ResultWeight.ShouldBeEquivalentTo(_primaryBuy.AccountingWeight - _secondaryBuy.AccountingWeight);
        }

        [Test]
        public void UnnettingBuyWithBuyDirectionChangingTest()
        {
            var unnettingResult = _calculator.GetUnnettingResult(_primaryBuy, new[] { _secondaryBuyDirectionChanging });
            unnettingResult.ResultWeight.ShouldBeEquivalentTo(_primaryBuy.AccountingWeight - _secondaryBuyDirectionChanging.AccountingWeight);
            unnettingResult.DirectionChanging.Should().BeTrue();
        }

        [Test]
        public void UnnettingBuyWithSellTest()
        {
            var unnettingResult = _calculator.GetUnnettingResult(_primaryBuy, new[] { _secondarySell });
            unnettingResult.ResultWeight.ShouldBeEquivalentTo(_primaryBuy.AccountingWeight + _secondarySell.AccountingWeight);
        }

        [Test]
        public void UnnettingSellWithSellTest()
        {
            var unnettingResult = _calculator.GetUnnettingResult(_primarySell, new[] { _secondarySell });
            unnettingResult.ResultWeight.ShouldBeEquivalentTo(-_primarySell.AccountingWeight + _secondarySell.AccountingWeight);
        }

        [Test]
        public void UnnettingSellWithSellDirectionChangingTest()
        {
            var unnettingResult = _calculator.GetUnnettingResult(_primarySell, new[] { _secondarySellDirectionChanging });
            unnettingResult.ResultWeight.ShouldBeEquivalentTo(-_primarySell.AccountingWeight + _secondarySellDirectionChanging.AccountingWeight);
            unnettingResult.DirectionChanging.Should().BeTrue();
        }

        [Test]
        public void UnnettingSellWithBuyTest()
        {
            var unnettingResult = _calculator.GetUnnettingResult(_primarySell, new[] { _secondaryBuy });
            unnettingResult.ResultWeight.ShouldBeEquivalentTo(-_primarySell.AccountingWeight - _secondaryBuy.AccountingWeight);
        }

        [Test]
        public void NettingBuyWithAdjustmentTest()
        {
            var nettingResult = _calculator.GetNettingResult(_primaryBuy, new[] { _adjustment });
            nettingResult.ResultWeight.ShouldBeEquivalentTo(_primaryBuy.AccountingWeight);
            nettingResult.ResultImbalance.ShouldBeEquivalentTo(_primaryBuy.ImbalanceWeight +
                                                               _adjustment.AdjustmentWeight);
        }

        [Test]
        public void UnnettingBuyWithAdjustmentTest()
        {
            var unnettingResult = _calculator.GetUnnettingResult(_primaryBuy, new[] { _adjustment });
            unnettingResult.ResultWeight.ShouldBeEquivalentTo(_primaryBuy.AccountingWeight);
            unnettingResult.ResultImbalance.ShouldBeEquivalentTo(_primaryBuy.ImbalanceWeight -
                                                                 _adjustment.AdjustmentWeight);
        }

        [Test]
        public void NettingSellWithAdjustmentTest()
        {
            var unnettingResult = _calculator.GetNettingResult(_primarySell, new[] { _adjustment });
            unnettingResult.ResultWeight.ShouldBeEquivalentTo(-_primarySell.AccountingWeight);
            unnettingResult.ResultImbalance.ShouldBeEquivalentTo(_primarySell.ImbalanceWeight -
                                                                 _adjustment.AdjustmentWeight);
        }

        [Test]
        public void UnnettingSellWithAdjustmentTest()
        {
            var unnettingResult = _calculator.GetUnnettingResult(_primarySell, new[] { _adjustment });
            unnettingResult.ResultWeight.ShouldBeEquivalentTo(-_primarySell.AccountingWeight);
            unnettingResult.ResultImbalance.ShouldBeEquivalentTo(_primarySell.ImbalanceWeight +
                                                                 _adjustment.AdjustmentWeight);
        }

        [Test]
        public void SellWeightFormattingTest()
        {
            var sellNettingResult = new NettingWeightResult(_primarySell);
            sellNettingResult.TransferWeightAsString.ShouldBeEquivalentTo(
                $"{_primarySell.AccountingWeight:F4} {_primarySell.AccountingUnit.Name} - Sell");
        }

        [Test]
        public void BuyWeightFormattingTest()
        {
            var buyNettingResult = new NettingWeightResult(_primaryBuy);
            buyNettingResult.TransferWeightAsString.ShouldBeEquivalentTo(
                $"{_primarySell.AccountingWeight:F4} {_primaryBuy.AccountingUnit.Name} - Buy");
        }
    }
}