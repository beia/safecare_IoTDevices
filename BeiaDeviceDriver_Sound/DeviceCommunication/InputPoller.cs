using System;
using System.Threading;
using VideoOS.Platform.DriverFramework.Managers;
using VideoOS.Platform.DriverFramework.Utilities;

namespace Safecare.BeiaDeviceDriver_Sound
{
    /// <summary>
    /// This is an example of how events can be retrieved from a hardware supporting events through polling.
    /// </summary>
    internal class InputPoller
    {
        private bool _shuttingDown;
        private Lazy<Thread> _listenerThread;
        private byte[] _lastFrame;
        private readonly object _frameLock = new object();
        private readonly DeviceMessageHandler _messageHandler;
        private DateTime _lastUpdateTime;

        public bool ReadyToRun => !_shuttingDown;

        public void PushFrame(byte[] data)
        {
            lock (_frameLock)
            {
                _lastFrame = data;
            }
        }

        public byte[] PopFrame()
        {
            var frame = _lastFrame;
            lock (_frameLock)
            {
                _lastFrame = null;
            }

            return frame;
        }

        public InputPoller(IEventManager eventManager, BeiaDeviceDriver_SoundConnectionManager connectionManager,
            DeviceMessageHandler messageHandler)
        {
            _messageHandler = messageHandler;
        }

        public void Start()
        {
            Toolbox.Log.Trace("Input poller starting");

            _listenerThread = new Lazy<Thread>(() => new Thread(CommunicationThreadMainLoop)
            {
                Name = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "BeiaDeviceDriver_Sound driver listener thread for input events"),
            }, LazyThreadSafetyMode.ExecutionAndPublication);

            _listenerThread.Value.Start();
        }

        public void Stop()
        {
            Toolbox.Log.Trace("Input poller stopping");
            _shuttingDown = true;
        }

        private void CommunicationThreadMainLoop()
        {
            while (!_shuttingDown)
            {
                try
                {
                    if (_messageHandler.Initialized && _lastUpdateTime < _messageHandler.Data.Time)
                    {
                        string msg = _messageHandler.Data.Serialize();
                        PushFrame(BitmapUtils.BitmapToJpegBytes(BitmapUtils.ConvertTextToImage(msg)));
                        _lastUpdateTime = _messageHandler.Data.Time;
                    }

                    Thread.Sleep(250);      // We check every 250 ms
                }
                catch (Exception e)
                {
                    Toolbox.Log.LogError("Input poller", e.StackTrace);
                    _shuttingDown = true;

                    Thread.Sleep(3000);
                }
            }
        }
    }
}
