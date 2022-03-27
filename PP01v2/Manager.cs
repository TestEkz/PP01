using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PP01v2
{
    class Manager
    {
        private static PM01Entities _context;
        public static Frame MainFrame { get; set; }
        public static PM01Entities GetContext()
        {
            if (_context == null)
            {
                _context = new PM01Entities();
            }
            return _context;
        }
    }
}
