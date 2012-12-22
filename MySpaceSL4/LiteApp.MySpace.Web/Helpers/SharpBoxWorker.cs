using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using AppLimit.CloudComputing.SharpBox;

namespace LiteApp.MySpace.Web.Helpers
{
    public static class SharpBoxTaskManager
    {
        //BackgroundWorker _worker;
        //Queue<Action<CloudStorage>> _pendingTasks = new Queue<Action<CloudStorage>>();

        //public void PublishTask(Action<CloudStorage> task)
        //{

        //}

        //BackgroundWorker CreateWorker()
        //{
        //    _worker = new BackgroundWorker();
        //    _worker.DoWork += _worker_DoWork;
        //    return _worker;
        //}

        //void _worker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    var storage = SharpBoxSupport.OpenDropBoxStorage();
        //    while (_pendingTasks.Count > 0)
        //    {
        //        var task = _pendingTasks.Dequeue();
        //        task(storage);
        //    }
        //    storage.Close();
        //}
    }
}