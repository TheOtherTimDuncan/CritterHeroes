using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Features.Shared.Queries;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Features.Shared.Models
{
    public class PagingModel
    {
        private int _totalRows;
        private int _pageSize;

        private PagingModel()
        {
            this._pageSize = 25;
        }

        public PagingModel(int totalRows, PagingQuery query)
            : this()
        {
            ThrowIf.Argument.IsNull(query, nameof(query));

            this._totalRows = totalRows;
            this.CurrentPage = query.Page ?? 1;
            this.Query = query;

            CalculatePages();
        }

        /// <summary>
        /// The number of page links to display not counting the links for the first or last page
        /// </summary>
        public int PageChunk
        {
            get
            {
                return 3;
            }
        }

        /// <summary>
        /// The number of rows in the page
        /// </summary>
        public virtual int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
                CalculatePages();
            }
        }

        public int TotalPages
        {
            get;
            private set;
        }

        public int CurrentPage
        {
            get;
            private set;
        }

        public int FirstPage
        {
            get;
            private set;
        }

        public int LastPage
        {
            get;
            private set;
        }

        public int PreviousPage
        {
            get;
            private set;
        }

        public int NextPage
        {
            get;
            private set;
        }

        public bool ShowLeadingChunkBreak
        {
            get
            {
                return FirstPage != 2 && TotalPages >= PageChunk;
            }
        }

        public bool ShowTrailingChunkBreak
        {
            get
            {
                return LastPage != TotalPages - 1;
            }
        }

        public PagingQuery Query
        {
            get;
            private set;
        }

        private void CalculatePages()
        {
            TotalPages = _totalRows / PageSize;
            if (_totalRows % PageSize > 0)
            {
                TotalPages++;
            }

            if (TotalPages > PageChunk)
            {
                if (CurrentPage <= PageChunk)
                {
                    FirstPage = 2;
                }
                else if (CurrentPage == TotalPages)
                {
                    FirstPage = TotalPages - PageChunk;
                }
                else
                {
                    if (CurrentPage % PageChunk == 0)
                    {
                        FirstPage = CurrentPage - PageChunk + 1;
                    }
                    else
                    {
                        FirstPage = ((((CurrentPage / PageChunk) + 1) * PageChunk) - PageChunk);
                    }
                }

                if (FirstPage < 2)
                {
                    FirstPage = 2;
                }

                LastPage = FirstPage + PageChunk - 1;
                if (LastPage >= TotalPages)
                {
                    LastPage = TotalPages - 1;
                }
            }
            else
            {
                FirstPage = 2;
                LastPage = TotalPages - 1;
            }

            PreviousPage = CurrentPage - 1;
            if (PreviousPage < 1)
            {
                PreviousPage = 1;
            }

            NextPage = CurrentPage + 1;
            if (NextPage > TotalPages)
            {
                NextPage = TotalPages;
            }
        }
    }
}
