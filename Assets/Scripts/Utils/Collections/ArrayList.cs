using System;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;

namespace Utils
{
    /// <summary>
    /// 一个可以直接获取到底层数组的 List.
    /// </summary>
    public class ArrayList<T> : IList<T>
    {
        /// <summary>
        /// 内部数据数组.
        /// </summary>
        T[] data;
        
        /// <summary>
        /// 元素计数.
        /// </summary>
        int cnt;
        
        public T this[int k]
        { 
            get => data[k];
            set => data[k] = value;
        }
        
        public T this[long k]
        { 
            get => data[k];
            set => data[k] = value;
        }
        
        public T this[uint k]
        { 
            get => data[k];
            set => data[k] = value;
        }
        
        public T this[ulong k]
        { 
            get => data[k];
            set => data[k] = value;
        }
        

        public int Count => cnt;

        public bool IsReadOnly => false;

        public void Add(T x)
        {
            if(data.Length == cnt) Resize(cnt * 2);
            data[cnt++] = x;
        }

        public void Clear()
        {
            cnt = 0;
        }

        public bool Contains(T item)
        {
            foreach(var i in data.ToEnumerable(0, cnt)) if(item.Equals(i)) return true;
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for(int i = 0; i < cnt; i++) array[i + arrayIndex] = data[i];
        }

        public IEnumerator<T> GetEnumerator() => data.ToEnumerable(0, cnt).GetEnumerator();

        public int IndexOf(T item)
        {
            for(int i = 0; i < cnt; i++) if(item.Equals(data[i])) return i;
            return -1;
        }

        public void Insert(int index, T item)
        {
            if(data.Length == cnt) Resize(cnt * 2);
            for(int i = cnt - 1; i >= index; i--) data[i + 1] = data[i];
            data[index] = item;
            cnt++;
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if(index == -1) return false;
            RemoveAt(index);
            return true;
        }

        public void RemoveAt(int index)
        {
            for(int i = index; i < cnt; i++) data[i] = data[i + 1];
            cnt--;
        }

        IEnumerator IEnumerable.GetEnumerator() => throw new NotSupportedException();
        
        // ====================================================================
        // 扩展功能.
        // ====================================================================
        
        public T[] array => data;
        
        public ArrayList<T> Reserve(int size)
        {
            if(size > data.Length) Resize(size);
            return this;
        }
        
        // ====================================================================
        // 内部工具函数
        // ====================================================================
        
        /// <summary>
        /// 重新申请此大小的数组.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Resize(int cnt)
        {
            var cdata = data;
            data = new T[cnt];
            var size = Math.Max(cdata.Length, cnt);
            for(int i = 0; i < size; i++) data[i] = cdata[i];
        }
    }
}
