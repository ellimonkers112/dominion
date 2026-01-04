using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominion.src.dominion.Dependencies.Encryption
{
    public class Asn1DerObject
    {
        // Token: 0x17000037 RID: 55
        // (get) Token: 0x060002C8 RID: 712 RVA: 0x00015E8D File Offset: 0x0001408D
        // (set) Token: 0x060002C9 RID: 713 RVA: 0x00015E95 File Offset: 0x00014095
        public Asn1Der.Type Type { get; set; }

        // Token: 0x17000038 RID: 56
        // (get) Token: 0x060002CA RID: 714 RVA: 0x00015E9E File Offset: 0x0001409E
        // (set) Token: 0x060002CB RID: 715 RVA: 0x00015EA6 File Offset: 0x000140A6
        public int Lenght { get; set; }

        // Token: 0x17000039 RID: 57
        // (get) Token: 0x060002CC RID: 716 RVA: 0x00015EAF File Offset: 0x000140AF
        public List<Asn1DerObject> Objects { get; }

        // Token: 0x1700003A RID: 58
        // (get) Token: 0x060002CD RID: 717 RVA: 0x00015EB7 File Offset: 0x000140B7
        // (set) Token: 0x060002CE RID: 718 RVA: 0x00015EBF File Offset: 0x000140BF
        public byte[] Data { get; set; }

        // Token: 0x060002CF RID: 719 RVA: 0x00015EC8 File Offset: 0x000140C8
        public Asn1DerObject()
        {
            this.Objects = new List<Asn1DerObject>();
        }

        // Token: 0x060002D0 RID: 720 RVA: 0x00015EDC File Offset: 0x000140DC
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            StringBuilder stringBuilder2 = new StringBuilder();
            Asn1Der.Type type = this.Type;
            switch (type)
            {
                case Asn1Der.Type.Integer:
                    {
                        foreach (byte b in this.Data)
                        {
                            stringBuilder2.AppendFormat("{0:X2}", b);
                        }
                        StringBuilder stringBuilder3 = stringBuilder;
                        string str = "\tINTEGER ";
                        StringBuilder stringBuilder4 = stringBuilder2;
                        stringBuilder3.AppendLine(str + ((stringBuilder4 != null) ? stringBuilder4.ToString() : null));
                        break;
                    }
                case (Asn1Der.Type)3:
                case (Asn1Der.Type)5:
                    break;
                case Asn1Der.Type.OctetString:
                    {
                        foreach (byte b2 in this.Data)
                        {
                            stringBuilder2.AppendFormat("{0:X2}", b2);
                        }
                        StringBuilder stringBuilder5 = stringBuilder;
                        string str2 = "\tOCTETSTRING ";
                        StringBuilder stringBuilder6 = stringBuilder2;
                        stringBuilder5.AppendLine(str2 + ((stringBuilder6 != null) ? stringBuilder6.ToString() : null));
                        break;
                    }
                case Asn1Der.Type.ObjectIdentifier:
                    {
                        foreach (byte b3 in this.Data)
                        {
                            stringBuilder2.AppendFormat("{0:X2}", b3);
                        }
                        StringBuilder stringBuilder7 = stringBuilder;
                        string str3 = "\tOBJECTIDENTIFIER ";
                        StringBuilder stringBuilder8 = stringBuilder2;
                        stringBuilder7.AppendLine(str3 + ((stringBuilder8 != null) ? stringBuilder8.ToString() : null));
                        break;
                    }
                default:
                    if (type == Asn1Der.Type.Sequence)
                    {
                        stringBuilder.AppendLine("SEQUENCE {");
                    }
                    break;
            }
            foreach (Asn1DerObject value in this.Objects)
            {
                stringBuilder.Append(value);
            }
            if (this.Type.Equals(Asn1Der.Type.Sequence))
            {
                stringBuilder.AppendLine("}");
            }
            return stringBuilder.ToString();
        }
    }
}
