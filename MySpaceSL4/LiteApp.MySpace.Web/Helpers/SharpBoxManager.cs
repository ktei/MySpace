using System;
using System.Collections.Generic;
using System.ComponentModel;
using AppLimit.CloudComputing.SharpBox;
using LiteApp.MySpace.Web.Logging;
using Ninject;

namespace LiteApp.MySpace.Web.Helpers
{
    public class SharpBoxTaskManager
    {
        BackgroundWorker _worker;
        Queue<Action<CloudStorage>> _pendingTasks = new Queue<Action<CloudStorage>>();

        public SharpBoxTaskManager()
        {
            DI.Inject(this);
        }

        public void PublishTask(Action<CloudStorage> task)
        {
            lock (_pendingTasks)
            {
                _pendingTasks.Enqueue(task);
            }
            if (_worker == null)
            {
                CreateWorker();
            }
            if (!_worker.IsBusy)
            {
                _worker.RunWorkerAsync();
            }
        }

        
        [Inject]
        public ILogger Logger { get; set; }

        BackgroundWorker CreateWorker()
        {
            _worker = new BackgroundWorker();
            _worker.DoWork += _worker_DoWork;
            _worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_worker_RunWorkerCompleted);
            return _worker;
        }

        void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var storage = SharpBoxSupport.OpenDropBoxStorage();
            try
            {
                while (_pendingTasks.Count > 0)
                {
                    var task = _pendingTasks.Dequeue();
                    task(storage);
                }
                storage.Close();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
            finally
            {
                if (storage != null)
                {
                    storage.Close();
                }
            }
        }
    }
}