using hesanta.FSM.States;
using hesanta.FSM.Transitions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace hesanta.FSM.tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class FSMTests
    {
        private IFSM fsm;


        [TestInitialize]
        public void Initialize()
        {
            fsm = new FSM();
        }

        [TestMethod]
        public void FSM_ConstructorWithNulls_ShouldThrowExceptions()
        {
            //Act
            void action() => new FSM();
            //Assert
            //Assert.ThrowsException<ArgumentNullException>(action);
        }

        [TestMethod]
        public void SetState_ShoudlSetAndExecuteTheState()
        {
            //Arrange
            Mock<IState> stateMock = new Mock<IState>();

            //Act
            fsm.SetState(stateMock.Object);

            //Assert
            Assert.AreEqual(fsm.CurrentState, stateMock.Object);
        }

        [TestMethod]
        public void AddStateTransition_ShouldAddTransitionBetweenStates()
        {
            //Arrange
            Mock<IState> stateMock = new Mock<IState>();
            Mock<IState> state2Mock = new Mock<IState>();
            Mock<ITransition> transitionMock = new Mock<ITransition>();

            //Act
            fsm.AddTransition(transitionMock.Object);
            void action() => fsm.AddTransition(transitionMock.Object);

            //Assert
            Assert.AreEqual(transitionMock.Object, fsm.Transitions.First());
            Assert.ThrowsException<InvalidOperationException>(action);
        }

        [TestMethod]
        public void RunAsync_WithoutAnyInitState_ShouldThrowsException()
        {
            //Act
            Task action() => fsm.RunAsync();

            //Assert
            Assert.AreEqual(FSMStatus.NotStarted, fsm.Status);
            Assert.ThrowsExceptionAsync<InvalidOperationException>(action);
        }

        [TestMethod]
        public void RunAsync_ShouldRunStatesAndTransitions()
        {
            //Arrange
            var startStateMock = new Mock<IStartState>();
            startStateMock.SetupGet(x => x.Execute).Returns(() => { });
            var moveStateMock = new Mock<IState>();
            moveStateMock.SetupGet(x => x.Execute).Returns(() => { });
            var endStateMock = new Mock<IEndState>();
            endStateMock.SetupGet(x => x.Execute).Returns(() => { });

            var startToMoveTransitionMock = new Mock<ITransition>();
            startToMoveTransitionMock.SetupGet(x => x.StateFrom).Returns(startStateMock.Object);
            startToMoveTransitionMock.SetupGet(x => x.StateTo).Returns(moveStateMock.Object);
            startToMoveTransitionMock.SetupGet(x => x.EvaluateFunc).Returns(() => moveStateMock.Object);
            var moveToEndTransitionMock = new Mock<ITransition>();
            moveToEndTransitionMock.SetupGet(x => x.StateFrom).Returns(moveStateMock.Object);
            moveToEndTransitionMock.SetupGet(x => x.StateTo).Returns(endStateMock.Object);
            moveToEndTransitionMock.SetupGet(x => x.EvaluateFunc).Returns(() => endStateMock.Object);
            fsm.AddTransitions(startToMoveTransitionMock.Object, moveToEndTransitionMock.Object);
            //Act
            fsm.RunAsync().Wait();

            //Assert
            Assert.AreEqual(FSMStatus.Finished, fsm.Status);
            startStateMock.Verify(x => x.Execute);
            moveStateMock.Verify(x => x.Execute);
            endStateMock.Verify(x => x.Execute);
        }

        [TestMethod]
        public void TestMethod1()
        {
            //Arrange

            //Act

            //Assert

        }
    }
}
