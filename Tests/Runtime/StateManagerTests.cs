using NUnit.Framework;
using StateManager;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests.Runtime
{
    public class TestStateManagerTests
    {
        private TestStateA stateA;
        private TestStateB stateB;

        private TestStateManager stateManager;

        [SetUp]
        public void Setup()
        {
            stateManager = new TestStateManager();
            stateA = new TestStateA(stateManager);
            stateB = new TestStateB(stateManager);
        }

        [TearDown]
        public void TearDown()
        {
            stateManager = null;
            stateA = null;
            stateB = null;
        }

        [Test]
        public void Transitions_To_StateA()
        {
            Assert.IsFalse(stateA.IsActive);
            stateManager.Transition<TestStateA>();
            Assert.IsTrue(stateA.IsActive);
        }

        [Test]
        public void Transitions_From_StateA_To_StateA()
        {
            Assert.IsFalse(stateA.IsActive);
            stateManager.Transition<TestStateA>();
            Assert.IsTrue(stateA.IsActive);
            stateManager.Transition<TestStateA>();
            Assert.IsTrue(stateA.IsActive);
        }

        [Test]
        public void Transitions_From_StateA_To_StateB()
        {
            Assert.IsFalse(stateA.IsActive);
            stateManager.Transition<TestStateA>();
            Assert.IsTrue(stateA.IsActive);
            stateManager.Transition<TestStateB>();
            Assert.IsFalse(stateA.IsActive);
            Assert.IsTrue(stateB.IsActive);
        }

        private abstract class TestState : State<TestState>
        {
            protected TestState(StateManager<TestState> stateManager)
                : base(stateManager)
            {
            }

            public bool IsActive { get; set; }
        }

        private sealed class TestStateA : TestState
        {
            public TestStateA(StateManager<TestState> stateManager)
                : base(stateManager)
            {
            }
        }

        private sealed class TestStateB : TestState
        {
            public TestStateB(StateManager<TestState> stateManager)
                : base(stateManager)
            {
            }
        }

        private sealed class TestStateManager : StateManager<TestState>
        {
            protected override void Transition(TestState newState)
            {
                if (CurrentState != null)
                    CurrentState.IsActive = false;
                base.Transition(newState);
                if (CurrentState != null)
                    CurrentState.IsActive = true;
            }
        }
    }
}