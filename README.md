# DoubleDoubleAdvancedIntegrate
 Double-Double Advanced Numerical Integration Implements 

## Requirement
.NET 8.0  
[DoubleDouble](https://github.com/tk-yoshimura/DoubleDouble)  
[DoubleDoubleComplex](https://github.com/tk-yoshimura/DoubleDoubleComplex)  
[DoubleDoubleIntegrate](https://github.com/tk-yoshimura/DoubleDoubleIntegrate)  

## Install

[Download DLL](https://github.com/tk-yoshimura/DoubleDoubleAdvancedIntegrate/releases)  
[Download Nuget](https://www.nuget.org/packages/tyoshimura.doubledouble.advancedintegrate/)  

## Usage

### Line Integral

```csharp
// Line integral on the line with integrand f(x, y) = x + 2 y
(ddouble value, ddouble error, long eval_points) = LineIntegral.AdaptiveIntegrate(
    (x, y) => x + 2 * y,
    Line2D.Line((0, 1), (2, 3)),
    Interval.Unit, eps: 0, maxdepth: 16
);
```

```csharp
// Line integral on the (t, t^2) with integrand f(x, y) = x + 2 y
(ddouble value, ddouble error, long eval_points) = LineIntegral.AdaptiveIntegrate(
    (x, y) => x + 2 * y,
    new Line2D(t => (t, t * t), t => (1, 2 * t)),
    (0, 1), eps: 0, maxdepth: 16
);
```

```csharp
// Line integral on the unit circle with integrand f(x, y) = x^2 + y^2
(ddouble value, ddouble error, long eval_points) = LineIntegral.AdaptiveIntegrate(
    (x, y) => x * x + y * y,
    Line2D.Circle,
    Interval.OmniAzimuth, eps: 0, maxdepth: 16
);
```

```csharp
// Ellipse circumference
(ddouble value, ddouble error, long eval_points) = LineIntegral.AdaptiveIntegrate(
    (x, y) => 1,
    Line2D.Circle * (1.5, 2),
    Interval.OmniAzimuth, eps: 0, maxdepth: 16
);
```

```csharp
// Line integral on the helix with integrand f(x, y, z) = z^2
(ddouble value, ddouble error, long eval_points) = LineIntegral.AdaptiveIntegrate(
    (x, y, z) => z * z,
    Line3D.Helix,
    Interval.OmniAzimuth, eps: 0, maxdepth: 16
);
```

```csharp
// Line integral on the unit circle (z = 1) with integrand f(x, y, z) = x^2 + y^2 + z
(ddouble value, ddouble error, long eval_points) = LineIntegral.AdaptiveIntegrate(
    (x, y, z) => x * x + y * y + z,
    Line3D.Circle + (0, 0, 1),
    Interval.OmniAzimuth, eps: 0, maxdepth: 16
);
```

### Surface Integral
```csharp
// Surface integral on the [0, 2]x[0, 1] with integrand f(x, y) = (x - y)^2
(ddouble value, ddouble error, long eval_points) = SurfaceIntegral.AdaptiveIntegrate(
    (x, y) => ddouble.Square(x - y),
    Surface2D.Ortho,
    (0, 2), (0, 1), eps: 0, maxdepth: 4
);
```

```csharp
// Surface integral on the triangle with integrand f(x, y) = 2 x + y
(ddouble value, ddouble error, long eval_points) = SurfaceIntegral.AdaptiveIntegrate(
    (x, y) => 2 * x + y,
    Surface2D.Triangle((0, 0), (1, 0), (0, 1)),
    Interval.Unit, Interval.Unit, eps: 0, maxdepth: 4
);
```

```csharp
// 2-dimensional Gaussian integration in polar coordinates
(ddouble value, ddouble error, long eval_points) = SurfaceIntegral.AdaptiveIntegrate(
    (x, y) => ddouble.Exp(-(x * x + y * y)),
    Surface2D.InfinityCircle,
    Interval.Unit, Interval.OmniAzimuth, eps: 0, maxdepth: 4
);
```

```csharp
// Surface integral on the unit sphere with integrand f(x, y, z) = x^2 + y + z^3
(ddouble value, ddouble error, long eval_points) = SurfaceIntegral.AdaptiveIntegrate(
    (x, y, z) => x * x + y + z * z * z,
    Surface3D.Sphere,
    Interval.OmniAltura, Interval.OmniAzimuth, eps: 0, maxdepth: 4
);
```

```csharp
// Surface integral on the unit sphere and (x > 0) with integrand f(x, y, z) = x^2 + y + z^3
(ddouble value, ddouble error, long eval_points) = SurfaceIntegral.AdaptiveIntegrate(
    (x, y, z) => x * x + y + z * z * z,
    Surface3D.Rotate(Surface3D.Sphere, (0, 0, 1), (1, 0, 0)),
    (0, ddouble.PI / 2), Interval.OmniAzimuth, eps: 0, maxdepth: 4
);
```

### Volume Integral
```csharp
// Volume integral on the ellipsoid with integrand f(x, y, z) = x^2 + y
(ddouble value, ddouble error, long eval_points) = VolumeIntegral.AdaptiveIntegrate(
    (x, y, z) => x * x + y,
    Volume3D.Sphere * (2, 3, 5),
    Interval.Unit, Interval.OmniAltura, Interval.OmniAzimuth,
    eps: 0, maxdepth: 2
);
```

### Complex Integral
note: If the integral path contains poles, the accuracy of the calculation results cannot be guaranteed.
```csharp
(Complex value, ddouble error, long eval_points) = ComplexIntegral.AdaptiveIntegrate(
    z => 1 / z,
    Line2D.Circle,
    Interval.OmniAzimuth, eps: 0, maxdepth: 16
);
```

## Licence
[MIT](https://github.com/tk-yoshimura/DoubleDoubleAdvancedIntegrate/blob/main/LICENSE)

## Author

[T.Yoshimura](https://github.com/tk-yoshimura)
