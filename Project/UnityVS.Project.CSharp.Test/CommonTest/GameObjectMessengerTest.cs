using System;

using Assets.Scripts.Common;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnityVS.Project.CSharp.Test.CommonTest
{
    [TestClass]
    public class GameObjectMessengerTest
    {
        [TestMethod]
        public void Register_Handler_Work()
        {
            var recipient = new object();

            var messenger = new GameObjectMessenger();
            messenger.Register<Message1>(recipient, x => { });
        }

        [TestMethod]
        public void Register_Two_Handler_Work()
        {
            var recipient = new object();

            var messenger = new GameObjectMessenger();
            messenger.Register<Message1>(recipient, x => { });
            messenger.Register<Message1>(recipient, x => { });
        }

        [TestMethod]
        public void Register_Two_Handler_Of_Two_Types_Work()
        {
            var recipient = new object();

            var messenger = new GameObjectMessenger();
            messenger.Register<Message1>(recipient, x => { });
            messenger.Register<Message1>(recipient, x => { });
            messenger.Register<Message2>(recipient, x => { });
            messenger.Register<Message2>(recipient, x => { });
        }

        [TestMethod]
        public void Unregister_Removes_Handler_Work()
        {
            var recipient = new object();
            Action<Message1> action = x => { };

            var messenger = new GameObjectMessenger();
            messenger.Register(recipient, action);
            messenger.Unregister(action);
        }

        [TestMethod]
        public void Send_Work()
        {
            var testObject = string.Empty;

            var recipient = new object();
            Action<Message1> action = x => { testObject = "Test Succesfull"; };

            var messenger = new GameObjectMessenger();
            messenger.Register(recipient, action);
            messenger.Send(new Message1());

            Assert.IsFalse(string.IsNullOrEmpty(testObject));
        }

        [TestMethod]
        public void Send_With_Unregistered_Type_Work()
        {
            var testObject = string.Empty;

            var recipient = new object();
            Action<Message1> action = x => { testObject = "Test Succesfull"; };

            var messenger = new GameObjectMessenger();
            messenger.Register(recipient, action);
            messenger.Send(new Message2());

            Assert.IsTrue(string.IsNullOrEmpty(testObject));
        }

        private class Message1
        {
        }

        private class Message2
        {
        }
    }
}
