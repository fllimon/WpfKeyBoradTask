using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    public class KeyboardPanel : Panel
    {
        public static DependencyProperty LineBreakBeforeProperty;

        static KeyboardPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KeyboardPanel), 
                        new FrameworkPropertyMetadata(typeof(KeyboardPanel)));
            FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata();
            metadata.AffectsArrange = true;
            metadata.AffectsMeasure = true;
            LineBreakBeforeProperty = DependencyProperty.RegisterAttached("LineBreakBefore", typeof(bool), typeof(KeyboardPanel), metadata);

        }

        public static void SetLineBreakBefore(UIElement element, bool value)
        {
            element.SetValue(LineBreakBeforeProperty, value);
        }

        public static bool GetLineBreakBefore(UIElement element)
        {
            return (bool)element.GetValue(LineBreakBeforeProperty);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size currentLineSize = new Size();
            Size panelSize = new Size();

            foreach (UIElement element in InternalChildren)
            {
                element.Measure(availableSize);
                Size desiredSize = element.DesiredSize;

                if (GetLineBreakBefore(element) || currentLineSize.Width + desiredSize.Width > availableSize.Width)
                {
                    panelSize.Width = Math.Max(currentLineSize.Width, panelSize.Width);
                    panelSize.Height += currentLineSize.Height;
                    currentLineSize = desiredSize;

                    if (desiredSize.Width > availableSize.Width)
                    {
                        panelSize.Width = Math.Max(desiredSize.Width, panelSize.Width);
                        panelSize.Height += desiredSize.Height;
                        currentLineSize = new Size();
                    }
                }
                else
                {
                    // Keep adding to the current line.
                    currentLineSize.Width += desiredSize.Width;

                    currentLineSize.Height = Math.Max(desiredSize.Height, currentLineSize.Height);
                }
            }

            panelSize.Width = Math.Max(currentLineSize.Width, panelSize.Width);
            panelSize.Height += currentLineSize.Height;

            return panelSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            int firstInLine = 0;

            Size currentLineSize = new Size();

            double accumulatedHeight = 0;

            UIElementCollection elements = InternalChildren;

            for (int i = 0; i < elements.Count; i++)
            {

                Size desiredSize = elements[i].DesiredSize;

                if (GetLineBreakBefore(elements[i]) || currentLineSize.Width + desiredSize.Width > finalSize.Width)
                {
                    ArrangeLine(accumulatedHeight, currentLineSize.Height, firstInLine, i);

                    accumulatedHeight += currentLineSize.Height;
                    currentLineSize = desiredSize;

                    if (desiredSize.Width > finalSize.Width)
                    {
                        ArrangeLine(accumulatedHeight, desiredSize.Height, i, ++i);
                        accumulatedHeight += desiredSize.Height;
                        currentLineSize = new Size();
                    }
                    firstInLine = i;
                }
                else
                {
                    currentLineSize.Width += desiredSize.Width;
                    currentLineSize.Height = Math.Max(desiredSize.Height, currentLineSize.Height);
                }
            }

            if (firstInLine < elements.Count)
                ArrangeLine(accumulatedHeight, currentLineSize.Height, firstInLine, elements.Count);

            return finalSize;
        }

        private void ArrangeLine(double y, double lineHeight, int start, int end)
        {
            double x = 0;
            UIElementCollection children = InternalChildren;

            for (int i = start; i < end; i++)
            {
                UIElement child = children[i];
                child.Arrange(new Rect(x, y, child.DesiredSize.Width, lineHeight));
                x += child.DesiredSize.Width;
            }
        }
    }
}
