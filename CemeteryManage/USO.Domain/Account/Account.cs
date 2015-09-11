namespace USO.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// �ͻ��˻���Ϣ
    /// </summary>
    public class Account
    {
        public Account()
        {
            CreatedOn = DateTime.UtcNow;
            ModifiedOn = DateTime.UtcNow;
            Organizations=new List<Organization>();
            AccountFiles=new List<AccountFile>();
            Products=new List<Product>();
           
        }


        [Key]
        public long Id { get; set; }

        /// <summary>
        /// �ͻ�����
        /// </summary>
        [StringLength(255)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// R3�ͻ�����
        /// </summary>
        [StringLength(100)]
        [Required]
        public string R3Code { get; set; }

        /// <summary>
        /// �ͻ�����
        /// </summary>
        [StringLength(1000)]
        public string Description { get; set; }

        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// �ͻ���֯��һ���ͻ��������ڶ����֯��
        /// </summary>
        public IList<Organization> Organizations { get; set; }

        /// <summary>
        /// �ͻ��ļ���һ���ͻ����Զ�Ӧ����ļ���
        /// </summary>
        public IList<AccountFile> AccountFiles { get; set; }

        /// <summary>
        /// �ͻ���־��һ���ͻ������ж�����־��
        /// </summary>
        public IList<AccountAnnotation> AccountAnnotations { get; set; }

        /// <summary>
        /// �ͻ���Ʒ��һ���ͻ�����ָ�������Ʒ��
        /// </summary>
        public IList<Product> Products { get; set; }

    }
}