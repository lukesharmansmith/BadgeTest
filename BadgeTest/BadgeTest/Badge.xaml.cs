namespace BadgeTest
{
    using SkiaSharp;
    using SkiaSharp.Views.Forms;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Badge : ContentView
    {
        public static readonly BindableProperty BadgeColourProperty = BindableProperty.Create(
            propertyName: nameof(Colour),
            returnType: typeof(Color),
            declaringType: typeof(Color),
            defaultValue: Color.Red,
            propertyChanged: OnPropertyChanged);

        public static readonly BindableProperty NumberProperty = BindableProperty.Create(
            propertyName: nameof(Number),
            returnType: typeof(int),
            declaringType: typeof(int),
            defaultValue: 0,
            propertyChanged: OnPropertyChanged);

        public static readonly BindableProperty TextColourProperty = BindableProperty.Create(
            propertyName: nameof(TextColour),
            returnType: typeof(Color),
            declaringType: typeof(Color),
            defaultValue: Color.White,
            propertyChanged: OnPropertyChanged);

        public Badge()
        {
            InitializeComponent();
        }

        public Color Colour
        {
            get => (Color)GetValue(BadgeColourProperty);
            set => SetValue(BadgeColourProperty, value);
        }

        public Color TextColour
        {
            get => (Color)GetValue(TextColourProperty);
            set => SetValue(TextColourProperty, value);
        }

        public int Number
        {
            get => (int)GetValue(NumberProperty);
            set => SetValue(NumberProperty, value);
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            var info = args.Info;
            var surface = args.Surface;
            var canvas = surface.Canvas;

            canvas.Clear();

            var backGroundPaint = new SKPaint
            {
                Style = SKPaintStyle.StrokeAndFill,
                Color = this.Colour.ToSKColor(),
                IsAntialias = true
            };

            var textPaint = new SKPaint
            {
                Style = SKPaintStyle.StrokeAndFill,
                Color = this.TextColour.ToSKColor(),
                IsAntialias = true
            };

            canvas.DrawCircle(info.Rect.MidX, info.Rect.MidY, info.Rect.MidY, backGroundPaint);
            
            if (this.Number > 0)
            {
                var badgeText = this.Number.ToString();

                var textWidth = textPaint.MeasureText(badgeText);
                textPaint.TextSize = 0.6f * info.Width * textPaint.TextSize / textWidth;

                var textBounds = new SKRect();
                textPaint.MeasureText(badgeText, ref textBounds);

                float xText = info.Rect.MidX - textBounds.MidX;
                float yText = info.Rect.MidY - textBounds.MidY;

                canvas.DrawText(badgeText, xText, yText, textPaint);
            }
        }
        private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            // Force a redraw if a property has changed
            if (bindable is Badge objCast)
            {
                objCast.canvas.InvalidateSurface();
            }
        }
    }
}