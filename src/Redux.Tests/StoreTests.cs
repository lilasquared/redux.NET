using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Redux.Tests
{
    [TestFixture]
    public class StoreTests
    {
        [Test]
        public void Should_push_initial_state()
        {
            var sut = new Store<int>(Reducers.PassThrough, 1);
            int stateReceived = 0;

            sut.StateChanged += state => stateReceived = state;

            Assert.AreEqual(1, stateReceived);
        }

        [Test]
        public void Should_push_state_on_dispatch()
        {
            var sut = new Store<int>(Reducers.Replace, 1);

            int stateReceived = 0;

            sut.StateChanged += state => stateReceived = state;
            sut.Dispatch(new FakeAction<int>(2));

            Assert.AreEqual(2, stateReceived);
        }

        [Test]
        [Ignore("This behavior should be moved to ApplyReducersTests")]
        public void Middleware_should_be_called_for_each_action_dispatched()
        {
            var numberOfCalls = 0;
            Middleware<int> spyMiddleware = store => next => action =>
            {
                numberOfCalls++;
                return next(action);
            };

            var sut = new Store<int>(Reducers.Replace, 1, spyMiddleware);
            
            sut.Dispatch(new FakeAction<int>(2));

            Assert.AreEqual(1, numberOfCalls);
            Assert.AreEqual(2, sut.State);
        }

        [Test]
        [Ignore("This behavior will be handle with a store enhancer in the next release")]
        public void Should_push_state_to_end_of_queue_on_nested_dispatch()
        {
            var sut = new Store<int>(Reducers.Replace, 1);
            sut.StateChanged += val =>
            {
                if (val < 5)
                {
                    sut.Dispatch(new FakeAction<int>(val + 1));
                }
            };

            Assert.AreEqual(5, sut.State);
        }

        [Test]
        public void State_should_return_initial_state()
        {
            var sut = new Store<int>(Reducers.Replace, 1);

            Assert.AreEqual(1, sut.State);
        }

        [Test]
        public void GetState_should_return_the_latest_state()
        {
            var sut = new Store<int>(Reducers.Replace, 1);

            sut.Dispatch(new FakeAction<int>(2));

            Assert.AreEqual(2, sut.State);
        }

        [Test]
        public async Task Store_should_be_thread_safe()
        {
            var sut = new Store<int>((state,action) => state + 1, 0);

            await Task.WhenAll(Enumerable.Range(0, 1000)
                .Select(_ => Task.Factory.StartNew(() => sut.Dispatch(new FakeAction<int>(0)))));

            Assert.AreEqual(1000, sut.State);
        }
    }
}
