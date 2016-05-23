using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Forum.Models
{
    public class PostModel
    {
        public int ID { get; set; }
        public DateTime DataUtworzenia { get; set; }
        public DateTime DataModyfikacji { get; set; }

        [DataType(DataType.MultilineText)]
        public string Tekst { get; set; }

        public int Temat_ID { get; set; }
        public string AutorID { get; set; }

        public virtual TematModel Temat { get; set; }
        public virtual ApplicationUser Autor { get; set; }  
    }

    public class TematModel
    {
        public int ID { get; set; }
        public string Tytul { get; set; }
        public DateTime DataUtworzenia { get; set; }
        public int Forum_ID { get; set; }

        public virtual ICollection<PostModel> Posty { get; set; }
        public virtual ForumModel Forum { get; set; }
    }

    public class ForumModel
    {
        public int ID { get; set; }
        public string Nazwa { get; set; }
        public string Opis { get; set; }

        public virtual ICollection<TematModel> Tematy { get; set; }
    }

    public class TworzenieTematuModel
    {
        public int fid { get; set; }
        public string Tytul { get; set; }

        [DataType(DataType.MultilineText)]
        public string Tekst { get; set; }
    }
}