using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INetCore.Core.Language.HTML
{
    public partial class CoreClass
    {
        public enum HTMLVersion
        {
			HTML2,
			HTML32,
			HTML4,
			HTML401,
			HTML5,
			XHTML1,
			XHTML1R,
			XHTML11,
			XHTML11SE
        }

        public enum XMLVersion
        {
			XML1,
			XML11
        }

        public enum CSSVersion
        {
			CSS1,
			CSS2,
			CSS3,
			CSS4
        }
    }
}
