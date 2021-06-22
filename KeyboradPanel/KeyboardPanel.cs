using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeyboradPanel
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:KeyboradPanel"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:KeyboradPanel;assembly=KeyboradPanel"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class KeyboardPanel :  StackPanel
    {
        static KeyboardPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KeyboardPanel), new FrameworkPropertyMetadata(typeof(KeyboardPanel)));
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Size panelSize = new Size();

            foreach (UIElement child in InternalChildren)
            {
                child.Measure(constraint);
                panelSize = child.DesiredSize;
            }

            return base.MeasureOverride(panelSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double x = 0;
            double y = 0;

            foreach (UIElement child in InternalChildren)
            {
                child.Arrange(new Rect(x, y, child.DesiredSize.Width, child.DesiredSize.Height));
            }

            return base.ArrangeOverride(new Size(x, finalSize.Height));
        }
    }
}
