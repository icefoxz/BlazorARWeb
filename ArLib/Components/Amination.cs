using System.Numerics;

namespace ArLib.Components;

public class Animation
{
    public enum AnimType
    {
        Simple,         // 使用 A-Frame 的 animation 组件
        AnimationMixer  // 使用 animation-mixer 组件
    }
    public enum EasingType
    {
        // Linear
        Linear,

        // Quadratic
        EaseInQuad,
        EaseOutQuad,
        EaseInOutQuad,

        // Cubic
        EaseInCubic,
        EaseOutCubic,
        EaseInOutCubic,

        // Quartic
        EaseInQuart,
        EaseOutQuart,
        EaseInOutQuart,

        // Quintic
        EaseInQuint,
        EaseOutQuint,
        EaseInOutQuint,

        // Sine
        EaseInSine,
        EaseOutSine,
        EaseInOutSine,

        // Exponential
        EaseInExpo,
        EaseOutExpo,
        EaseInOutExpo,

        // Circular
        EaseInCirc,
        EaseOutCirc,
        EaseInOutCirc,

        // Back
        EaseInBack,
        EaseOutBack,
        EaseInOutBack,

        // Elastic
        EaseInElastic,
        EaseOutElastic,
        EaseInOutElastic,
    }

    public enum PropertyType
    {
        Rotation,
        Position,
        Scale,
    }
    public enum DirType
    {
        Normal,
        Reverse,
        Alternate,
    }
    public AnimType Type { get; set; } = AnimType.Simple;

    // 适用于简单动画的属性
    public PropertyType? Property { get; set; }
    public string? From { get; set; }
    public string? To { get; set; }
    public int? Dur { get; set; }
    public EasingType? Easing { get; set; }
    public bool? Loop { get; set; }
    public DirType? Dir { get; set; }
    public string? Delay { get; set; }
    public string? Fill { get; set; }

    // 适用于 animation-mixer 的属性
    public string? Clip { get; set; } = "*";  // 默认播放所有动画
    public bool? LoopAnimation { get; set; } = true;
    public bool? Repetitions { get; set; } = true;
    public bool? CrossFadeDuration { get; set; }
    public bool? ClampWhenFinished { get; set; }
    public bool? UseRegExp { get; set; }

    // 生成属性字符串
    public string GetAttribute()
    {
        switch (Type)
        {
            case AnimType.Simple:
            {
                var parameters = new List<string>();
                if (Property.HasValue) parameters.Add($"property: {PropertyTypeToString(Property.Value)}");
                if (!string.IsNullOrEmpty(From)) parameters.Add($"from: {From}");
                if (!string.IsNullOrEmpty(To)) parameters.Add($"to: {To}");
                if (Dur.HasValue) parameters.Add($"dur: {Dur.Value}");
                if (Easing.HasValue) parameters.Add($"easing: {EasingTypeToString(Easing.Value)}");
                if (Loop.HasValue) parameters.Add($"loop: {(Loop.Value ? "true" : "false")}");
                if (Dir.HasValue) parameters.Add($"dir: {DirTypeToString(Dir.Value)}");
                if (!string.IsNullOrEmpty(Delay)) parameters.Add($"delay: {Delay}");
                if (!string.IsNullOrEmpty(Fill)) parameters.Add($"fill: {Fill}");

                return string.Join("; ", parameters);
            }
            case AnimType.AnimationMixer:
            {
                var parameters = new List<string>();
                if (!string.IsNullOrEmpty(Clip)) parameters.Add($"clip: {Clip}");
                if (LoopAnimation.HasValue) parameters.Add($"loop: {(LoopAnimation.Value ? "true" : "false")}");
                if (Repetitions.HasValue) parameters.Add($"repetitions: {Repetitions.Value}");
                if (CrossFadeDuration.HasValue) parameters.Add($"crossFadeDuration: {CrossFadeDuration.Value}");
                if (ClampWhenFinished.HasValue) parameters.Add($"clampWhenFinished: {(ClampWhenFinished.Value ? "true" : "false")}");
                if (UseRegExp.HasValue) parameters.Add($"useRegExp: {(UseRegExp.Value ? "true" : "false")}");

                return string.Join("; ", parameters);
            }
            default: return string.Empty;
        }
    }

    string DirTypeToString(DirType dir) => dir switch
    {
        DirType.Normal => "normal",
        DirType.Reverse => "reverse",
        DirType.Alternate => "alternate",
        _ => string.Empty
    };

    string PropertyTypeToString(PropertyType property)=> property switch
    {
        PropertyType.Rotation => "rotation",
        PropertyType.Position => "position",
        PropertyType.Scale => "scale",
        _ => string.Empty
    };

    // 添加一个方法，将 EasingType 枚举值转换为字符串
    string EasingTypeToString(EasingType easingType)
    {
        return easingType switch
        {
            EasingType.Linear => "linear",
            EasingType.EaseInQuad => "easeInQuad",
            EasingType.EaseOutQuad => "easeOutQuad",
            EasingType.EaseInOutQuad => "easeInOutQuad",
            EasingType.EaseInCubic => "easeInCubic",
            EasingType.EaseOutCubic => "easeOutCubic",
            EasingType.EaseInOutCubic => "easeInOutCubic",
            EasingType.EaseInQuart => "easeInQuart",
            EasingType.EaseOutQuart => "easeOutQuart",
            EasingType.EaseInOutQuart => "easeInOutQuart",
            EasingType.EaseInQuint => "easeInQuint",
            EasingType.EaseOutQuint => "easeOutQuint",
            EasingType.EaseInOutQuint => "easeInOutQuint",
            EasingType.EaseInSine => "easeInSine",
            EasingType.EaseOutSine => "easeOutSine",
            EasingType.EaseInOutSine => "easeInOutSine",
            EasingType.EaseInExpo => "easeInExpo",
            EasingType.EaseOutExpo => "easeOutExpo",
            EasingType.EaseInOutExpo => "easeInOutExpo",
            EasingType.EaseInCirc => "easeInCirc",
            EasingType.EaseOutCirc => "easeOutCirc",
            EasingType.EaseInOutCirc => "easeInOutCirc",
            EasingType.EaseInBack => "easeInBack",
            EasingType.EaseOutBack => "easeOutBack",
            EasingType.EaseInOutBack => "easeInOutBack",
            EasingType.EaseInElastic => "easeInElastic",
            EasingType.EaseOutElastic => "easeOutElastic",
            EasingType.EaseInOutElastic => "easeInOutElastic",
            _ => "linear"
        };
    }
}