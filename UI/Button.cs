using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reaper.UI;

[RequireModule(typeof(RectTransform))]
public class Button : EntityModule, IClickable
{
    public event Action OnClick;

    public void Click()
    {
        OnClick?.Invoke();
    }
}
