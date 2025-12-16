using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUpload.Windows
{
    public class ProductControl<T> : UserControl
    {
        public Image BaseImage { get; protected set; }

        public Image BaseBackImage { get; protected set; }

        public Image ArtImage { get; set; }

        public Image BackArtImage { get; set; }

        public string FrontLeftAlign { get; set; }

        public string FrontTopAlign { get; set; }

        public string BackLeftAlign { get; set; }

        public string BackTopAlign { get; set; }

        public List<T> SelectedColors { get; protected set; }

        public event EventHandler IsDefaultChanged;

        public event EventHandler SelectedColorsChanged;
        
        protected Color _shirtColor = Color.Transparent;

        protected bool _isFront;

        public ProductControl()
        {
            SelectedColors = new List<T>();
            _isFront = true;
        }

        protected virtual void OnIsDefaultChanged(EventArgs e)
        {
            IsDefaultChanged?.Invoke(this, e);
        }

        protected virtual void OnSelectedColorsChanged(EventArgs e)
        {
            SelectedColorsChanged?.Invoke(this, e);
        }
    }
}
