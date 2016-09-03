using System;
using System.Collections.Generic;
using System.Linq;

namespace Dickson.Core.ComponentModel
{
    public class Paging<T> //没有实现集合接口，因为对于集合Json.Net默认行为下会忽略掉其他属性。
    {
        List<T> m_InnerList;

        public Paging(int start, int take, int total, IEnumerable<T> source)
        {
            if (start < 0)
                throw new ArgumentOutOfRangeException("start");
            if (take <= 0)
                throw new ArgumentOutOfRangeException("take");
            if (total < 0)
                throw new ArgumentOutOfRangeException("total");

            Start = start;
            Take = take;
            Total = total;
            m_InnerList = new List<T>(source);
        }

        public int Start { get; private set; }

        public int Take { get; private set; }

        public int Total { get; private set; }

        public int CurrentPage
        {
            get
            {
                if (m_InnerList.Count > 0)
                    return (int)Math.Ceiling((double)(Start + 1) / Take);
                return 0;
            }
        }

        public int NextPage
        {
            get
            {
                if (m_InnerList.Count > 0 && HasNextPage)
                    return CurrentPage + 1;
                return CurrentPage;
            }
        }

        public int PreviousPage
        {
            get
            {
                if (m_InnerList.Count > 0 && HasPreviousPage)
                    return CurrentPage - 1;
                return CurrentPage;
            }
        }

        public int TotalPage
        {
            get
            {
                return (int)Math.Ceiling((double)Total / Take);
            }
        }

        public bool IsFirstPage
        {
            get
            {
                return CurrentPage == 1;
            }
        }

        public bool IsLastPage
        {
            get
            {
                if (m_InnerList.Count > 0)
                    return CurrentPage == TotalPage;
                return false;
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                if (m_InnerList.Count > 0)
                    return !IsFirstPage;
                return false;
            }
        }

        public bool HasNextPage
        {
            get
            {
                if (m_InnerList.Count > 0)
                    return !IsLastPage;
                return false;
            }
        }

        public IReadOnlyCollection<T> List
        {
            get { return m_InnerList; }
        }

        public int Count
        {
            get { return m_InnerList.Count; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_InnerList.GetEnumerator();
        }

        public Paging<TResult> ConvertTo<TResult>(Converter<T, TResult> convert)
        {
            if (convert == null)
                throw new ArgumentNullException("convert");

            var list = new Paging<TResult>(Start, Take, Total, m_InnerList.Select(item => convert(item)));
            return list;
        }

        public int GetStart(int page)
        {
            return (page - 1) * Take;
        }
    }
}
