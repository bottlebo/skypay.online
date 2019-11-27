using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyPay.Models
{
    public class Document
    {
        public int Id { get; set; }
        public DateTime DocumentDate { get; set; }
        public DocumentType Type { get; set; }
        public decimal makrkup { get; set; }
        public NdsValue nds { get; set; }
        public bool Locked { get; set; }
        public bool Private { get; set; }

        [ForeignKey("Agent")]
        public int? AgentId { get; set; }
        public Agent Agent { get; set; }
        [ForeignKey("Stock")]
        public int StockId { get; set; }
        public Stock Stock { get; set; }

        public virtual ICollection<DocumentItem> Items { get; set; }
        [NotMapped]
        public decimal Total {
            get
            {
                return 0;
            }
        }
        [NotMapped]
        public int Number
        {
            get { return Id; }
        }
    }
    public class DocumentItem
    {
        public int Id { get; set; }
        [ForeignKey("Document")]
        public int DocumentId { get; set; }
        public Document Document { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public decimal Qty { get; set; }
        public decimal InputPrice { get; set; }
        public decimal? OutputPrice { get; set; }

        [NotMapped]
        public decimal Summ {  get { return InputPrice * Qty; } }
    }
}
