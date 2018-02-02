using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace StripController.Infrastructure.StripWrapper
{
    class NetworkManager
    {
        private readonly UdpClient _client;
        private readonly List<Command> _commandQueue;
        private readonly AutoResetEvent _commanQueueEvent;
        private readonly object _commandQueueSync;
        private readonly Thread _commandThread;
        private bool _isActive;

        public NetworkManager()
        {
            _client = new UdpClient();
            _commanQueueEvent = new AutoResetEvent(false);
            _commandQueueSync = new object();
            _commandQueue = new List<Command>();
            _commandThread = new Thread(CommandThreadBody);
        }

        public void Start(string address, int port)
        {
            _client.Connect(address, port);
            _isActive = true;
            _commandThread.Start();
        }

        public void Stop()
        {
            _isActive = false;
            _commanQueueEvent.Set();
            _commandThread.Join();
        }

        public void EnqueueCommand(Command command)
        {
            lock (_commandQueueSync)
            {
                _commandQueue.Add(command);
                _commanQueueEvent.Set();
            }
        }

        private void CommandThreadBody()
        {
            while (_isActive)
            {
                _commanQueueEvent.WaitOne();

                Command[] commands;
                lock (_commandQueueSync)
                {
                    commands = _commandQueue.ToArray();
                    _commandQueue.Clear();
                }

                foreach (var command in commands)
                    SendCommand(command);
            }
        }

        private void SendCommand(Command command)
        {
            var commandBody = command.GetBody();
            var commandId = command.GetCommandId();

            var commandIdRaw = ArduinoTypes.GetBytes((byte)commandId);
            var commandLengthRaw = ArduinoTypes.GetBytes((byte)(commandBody.Length + commandIdRaw.Length));
            var message = new byte[commandBody.Length + commandIdRaw.Length + commandLengthRaw.Length];

            Array.Copy(commandLengthRaw, message, commandLengthRaw.Length);
            Array.Copy(commandIdRaw, 0, message, commandLengthRaw.Length, commandIdRaw.Length);
            Array.Copy(commandBody, 0, message, commandLengthRaw.Length + commandIdRaw.Length, commandBody.Length);

            _client.Send(message, message.Length);
        }
    }    
}
