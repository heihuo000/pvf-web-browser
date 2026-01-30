using System.Collections.Generic;
using pvfUtility.Interface.View;

namespace pvfUtility.Service
{
    /// <summary>
    ///     处理前进和后退的服务
    /// </summary>
    internal static class NavigationServices
    {
        //设置保存最大条数,当达到该条数时，每次增加记录时，均依次删除原有记录
        private const int MaxList = 20;
        private static LinkedListNode<IEditor> _historyCurrentNode;
        private static readonly LinkedList<IEditor> HistoryList = new LinkedList<IEditor>();

        public static bool HasCurrentHistory => _historyCurrentNode != null;

        /// <summary>
        ///     或取当前的记录信息
        /// </summary>
        public static IEditor CurrentHistory => _historyCurrentNode?.Value;

        /// <summary>
        ///     当前后退时否可用，用于设置按钮状态信息
        /// </summary>
        public static bool CanBack => HasCurrentHistory && _historyCurrentNode.Next != null;

        /// <summary>
        ///     当前前进时否可用，用于设置按钮状态信息
        /// </summary>
        public static bool CanForward => HasCurrentHistory && _historyCurrentNode.Previous != null;

        /// <summary>
        ///     向历史记录链表中加入新的节点
        /// </summary>
        /// <param name="h"></param>
        public static void Add(IEditor h)
        {
            var tem = HistoryList.First;
            //如果连续加入url相同的记录,则只加入一次,可以根据自已情况设置
            if (tem != null && tem.Value.Equals(h))
                return;

            //当当前节点不为空,或该节点的上一个节点也不为空时,则删除该节点的前所有节点
            //模拟IE对前进后退的处理
            if (_historyCurrentNode?.Previous != null)
                DelNode(_historyCurrentNode);

            //处理限制最大记录条数
            if (HistoryList.Count + 1 > MaxList)
                HistoryList.RemoveLast();

            _historyCurrentNode = new LinkedListNode<IEditor>(h);
            HistoryList.AddFirst(_historyCurrentNode);
        }

        /// <summary>
        ///     向历史记录链表中删除节点
        /// </summary>
        /// <param name="h"></param>
        public static void Remove(IEditor h)
        {
            var result = HistoryList.Find(h);
            if (result != null) HistoryList.Remove(result);
        }

        /// <summary>
        ///     后退
        /// </summary>
        public static void Back()
        {
            _historyCurrentNode = _historyCurrentNode.Next;
        }

        /// <summary>
        ///     前进
        /// </summary>
        public static void Forward()
        {
            _historyCurrentNode = _historyCurrentNode.Previous;
        }

        /// <summary>
        ///     删除指定节点前所有节点
        /// </summary>
        /// <param name="node"></param>
        private static void DelNode(LinkedListNode<IEditor> node)
        {
            while (node.Previous != null) HistoryList.Remove(node.Previous);
        }
    }
}