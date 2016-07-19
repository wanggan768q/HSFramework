using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HS.Base
{
    public static class HS_Scheduler
    {
        private class Task
        {
            public int id;
            public int repeat;
            // -1 : Enterframe, 0 : Repeat, >0 : RepeatCount
            public int interval;
            public int nextExecTime;
            public System.Action action;
            public bool ignoreTimeScale;
            public bool destroyed;

            public Task(System.Action action)
                : this(action, 0, -1, 0, true)
            {
            }

            public Task(System.Action action, int interval, int repeat, int nextExecTime, bool ignoreTimeScale)
            {
                this.id = ++mUniqueId;
                this.action = action;
                this.repeat = repeat;
                this.interval = interval;
                this.nextExecTime = nextExecTime;
                this.ignoreTimeScale = ignoreTimeScale;
                this.destroyed = false;
            }
        }

        private static int mUniqueId = 0;
        private static List<HS_Scheduler.Task> mTaskList = new List<HS_Scheduler.Task>();
        private static Dictionary<int, HS_Scheduler.Task> mTaskMap = new Dictionary<int, HS_Scheduler.Task>();
        private static Dictionary<System.Action, HS_Scheduler.Task> mTaskDict = new Dictionary<System.Action, HS_Scheduler.Task>();

        static HS_Scheduler()
        {
            HS_SchedulerExecutorBehaviour beh = HS_SchedulerExecutorBehaviour.GetInstance();
        }

        #region static variables and functions

        public static float GetTimer(bool ignoreTimeScale = true)
        {
            if (ignoreTimeScale)
            {
                return Time.realtimeSinceStartup;
            }
            return Time.time;
        }

        private static int EnterFrame(System.Action action)
        {
            return Add(action, 0, -1, true);
        }

        private static int Timeout(System.Action action, int interval = 0, bool ignoreTimeScale = true)
        {
            return Add(action, interval, 1, ignoreTimeScale);
        }

        private static int Interval(System.Action action, int interval, bool ignoreTimeScale = true)
        {
            return Add(action, interval, 0, ignoreTimeScale);
        }

        private static int Interval(System.Action action, int interval, int repeatCount, bool ignoreTimeScale = true)
        {
            return Add(action, interval, repeatCount, ignoreTimeScale);
        }

        private static bool Remove(ref int taskId)
        {
            HS_Scheduler.Task task;
            if (mTaskMap.TryGetValue(taskId, out task))
            {
                task.destroyed = true;
                mTaskDict.Remove(task.action);
                mTaskList.Remove(task);
                mTaskMap.Remove(taskId);
                taskId = 0;
                return true;
            }
            return false;
        }

        private static bool Complete(ref int taskId)
        {
            HS_Scheduler.Task task;
            if (mTaskMap.TryGetValue(taskId, out task))
            {
                task.action();
                Remove(ref taskId);
                return true;
            }
            return false;
        }

        private static bool Remove(System.Action action)
        {
            HS_Scheduler.Task task;
            if (mTaskDict.TryGetValue(action, out task))
            {
                task.destroyed = true;
                mTaskDict.Remove(action);
                mTaskList.Remove(task);
                mTaskMap.Remove(task.id);
                return true;
            }
            return false;
        }

        private static int Add(System.Action action, int interval, int repeatCount, bool ignoreTimeScale)
        {
            HS_Scheduler.Task task;
            if (mTaskDict.TryGetValue(action, out task))
            {
                return task.id;
            }
            task = new HS_Scheduler.Task(action, interval, repeatCount, (int)(GetTimer(ignoreTimeScale) * 1000) + interval, ignoreTimeScale);
            if (repeatCount == -1)
            {
                int index = 0;
                for (int i = 0, count = mTaskList.Count; i < count; i++)
                {
                    if (mTaskList[i].repeat != -1)
                    {
                        index = i;
                        break;
                    }
                }
                mTaskList.Insert(index, task);
            }
            else
            {
                InsertTask(task);
            }
            mTaskMap.Add(task.id, task);
            mTaskDict.Add(task.action, task);
            return task.id;
        }

        private static void InsertTask(HS_Scheduler.Task task)
        {
            int left = 0, right = mTaskList.Count, middle = right;
            while (left < right)
            {
                middle = (left + right) / 2;
                HS_Scheduler.Task obj = mTaskList[middle];
                if (obj.repeat == -1)
                {
                    left = middle + 1;
                }
                else if (task.nextExecTime >= obj.nextExecTime)
                {
                    left = middle + 1;
                }
                else if (task.nextExecTime < obj.nextExecTime)
                {
                    right = middle - 1;
                }
            }
            mTaskList.Insert(middle, task);
        }

        #endregion

        public class Proxy
        {
            private List<int> mTaskIds = null;

            public Proxy()
            {
                mTaskIds = new List<int>();
            }

            ~Proxy()
            {
                if (mTaskIds != null)
                {
                    Destroy();
                }
            }

            public int EnterFrame(System.Action action)
            {
                return Record(HS_Scheduler.Add(action, 0, -1, true));
            }

            public int Timeout(System.Action action, float interval = 0, bool ignoreTimeScale = true)
            {
                return Record(HS_Scheduler.Add(action, System.Convert.ToInt32(interval * 1000), 1, ignoreTimeScale));
            }

            public int Interval(System.Action action, float interval, bool ignoreTimeScale = true)
            {
                return Record(HS_Scheduler.Add(action, System.Convert.ToInt32(interval * 1000), 0, ignoreTimeScale));
            }

            public int Interval(System.Action action, float interval, int repeatCount, bool ignoreTimeScale = true)
            {
                return Record(HS_Scheduler.Add(action, System.Convert.ToInt32(interval * 1000), repeatCount, ignoreTimeScale));
            }

            public bool Complete(ref int taskId)
            {
                return HS_Scheduler.Complete(ref taskId);
            }

            public bool Remove(ref int taskId)
            {
                return HS_Scheduler.Remove(ref taskId);
            }

            public bool Remove(System.Action action)
            {
                return HS_Scheduler.Remove(action);
            }

            public void Destroy()
            {
                if (mTaskIds != null)
                {
                    for (int i = mTaskIds.Count - 1; i >= 0; i--)
                    {
                        int taskId = mTaskIds[i];
                        HS_Scheduler.Remove(ref taskId);
                    }
                    mTaskIds = null;
                }
            }

            private int Record(int taskId)
            {
                if (mTaskIds != null)
                {
                    mTaskIds.Add(taskId);
                }
                else
                {
                    Debug.Log("Task list has been destroy but you still access it." + taskId);
                    taskId = -1;
                }
                return taskId;
            }
        }

        public class HS_SchedulerExecutorBehaviour : HS_SingletonGameObject<HS_SchedulerExecutorBehaviour>
        {
            void Update()
            {
                int currentTimer = (int)(GetTimer() * 1000), currentTimerOnTimeScale = (int)(GetTimer(false));
                HS_Scheduler.Task[] tasks = mTaskList.ToArray();
                int index = 0, count = tasks.Length;
                for (; index < count; index++)
                {
                    HS_Scheduler.Task task = tasks[index];
                    if (task.destroyed)
                        continue;
                    if (task.repeat == -1)
                    {
                        task.action();
                    }
                    else
                    {
                        int timer = task.ignoreTimeScale ? currentTimer : currentTimerOnTimeScale;
                        if (timer >= task.nextExecTime)
                        {
                            mTaskList.Remove(task);
                            if (task.repeat > 0 && --task.repeat == 0)
                            {
                                mTaskMap.Remove(task.id);
                                mTaskDict.Remove(task.action);
                                task.destroyed = true;
                            }
                            task.action();
                            if (!task.destroyed)
                            {
                                task.nextExecTime = timer + task.interval;
                                InsertTask(task);
                            }
                        }
                    }
                }
            }
        }

    }
}

