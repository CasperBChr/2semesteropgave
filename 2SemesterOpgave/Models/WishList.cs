using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class WishList
    {
        public Article article;
        public User user;
        public DateTime Createdadd;

        public WishList(Article article, User user)
        {
            this.article = article;
            this.user = user;
            this.Createdadd = DateTime.Now;
        }
    }
}
