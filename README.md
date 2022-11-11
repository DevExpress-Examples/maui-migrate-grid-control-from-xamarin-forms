<!-- default badges list -->
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1127033)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
# Migrate a Xamarin.Forms Application to .NET MAUI

This repository illustrates how to migrate the [DevExpress Xamarin.Forms DataGrid Get Started application](https://github.com/DevExpress-Examples/xamarin-forms-data-grid-examples/tree/22.1.3%2B/CS/GettingStarted) to DevExpress .NET MAUI Controls. Refer to the [opened pull request](https://github.com/DevExpress-Examples/maui-migrate-grid-control-from-xamarin-forms/pull/1/) to see the changes that are we made to migrate the application to .NET MAUI platform.

To migrate a Xamarin.Forms application to .NET MAUI, you should perform the following steps:

* Convert the projects from .NET Framework to .NET SDK Style
* Update NuGet packages
* Update code namespaces
* Address any breaking API changes

## Step 1: Update the .csproj Files

The [maui_grid_get_started_migrated.csproj](https://github.com/DevExpress-Examples/maui-migrate-grid-control-from-xamarin-forms/pull/1/files#diff-0c0026324b1c4e828e8afa24df6ccf414fd9f1c2d2ed7c39f276e8973f510217) file includes the content of the .NET MAUI project that is migrated from the Xamarin.Forms.

Refer to the following Microsoft topic for more information on how to update the csproj files: [Migrating from Xamarin.Forms - csproj files updates](https://github.com/dotnet/maui/wiki/Migrating-from-Xamarin.Forms-(Preview)#step-1--csproj-files-updates).

## Step 2: Update the Framework and DevExpress Controls NuGet References

To reference .NET MAUI Framework and Controls instead of the Xamarin.Forms, replicate the changes made [maui_grid_get_started_migrated.csproj](https://github.com/DevExpress-Examples/maui-migrate-grid-control-from-xamarin-forms/pull/1/files#diff-0c0026324b1c4e828e8afa24df6ccf414fd9f1c2d2ed7c39f276e8973f510217R37) file:
```diff
+ <ItemGroup>
+   <PackageReference Include="DevExpress.Maui.DataGrid" Version="22.2.*" />
+   <PackageReference Include="DevExpress.Data" Version="22.2.*" />
+ </ItemGroup>
```

## Step 3: Source Code Updates

### Step 3.1: Update the Framework and DevExpress Control Namespaces

The [MainPage.xaml](
https://github.com/DevExpress-Examples/maui-migrate-grid-control-from-xamarin-forms/pull/1/files#diff-46feb52f587f3e1f6faaf4f23c928c93a67eccb05ddc2000db6303dc3f10ff62R2) file contains changes you should perform to migrate a project to .NET MAUI: 

  ```diff
  - xmlns="http://xamarin.com/schemas/2014/forms"
  - xmlns:dxg="http://schemas.devexpress.com/xamarin/2014/forms/datagrid"
  - xmlns:ios="clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core"
  + xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
  + xmlns:dxg="clr-namespace:DevExpress.Maui.DataGrid;assembly=DevExpress.Maui.DataGrid"
  + xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls"
  ```

You should also update the code-behind references in the [MainPage.xaml.cs](https://github.com/DevExpress-Examples/maui-migrate-grid-control-from-xamarin-forms/pull/1/files#diff-6eac6ac4461489f83db3a88bb5d9c2b916cddb8a933bd285d9704b555e5d4375R1) file:

  ```diff
  - using Xamarin.Forms;
  - using DevExpress.XamarinForms.DataGrid;
  + using DevExpress.Maui.DataGrid;
  + using DevExpress.Maui.Core;
  ```

### Step 3.2: Replace Initializers with the UseDevExpress Method

1. Remove the DataGrid Initializer.Init() method from the [MainPage.xaml.cs](https://github.com/DevExpress-Examples/maui-migrate-grid-control-from-xamarin-forms/pull/1/files#diff-6eac6ac4461489f83db3a88bb5d9c2b916cddb8a933bd285d9704b555e5d4375L9):
    ```diff
    public MainPage() {
    -  DevExpress.XamarinForms.DataGrid.Initializer.Init();
      InitializeComponent();
    }
    ```
1. Add the UseDevExpress method to the [MainProgram.cs](https://github.com/DevExpress-Examples/maui-migrate-grid-control-from-xamarin-forms/pull/1/files) file:
    ```diff
    public static MauiApp CreateMauiApp() {
       var builder = MauiApp.CreateBuilder();
       builder
           .UseMauiApp<App>()
    +      .UseDevExpress()
           .ConfigureFonts(fonts =>
              /// ...
           );
        /// ...
    }
    ```

### Step 3.3: Update Source Code to Address Breaking API Changes

In the [MainPage.xaml.cs](https://github.com/DevExpress-Examples/maui-migrate-grid-control-from-xamarin-forms/pull/1/files#diff-6eac6ac4461489f83db3a88bb5d9c2b916cddb8a933bd285d9704b555e5d4375L16) file, you need to replace the outdated [CustomSummaryProcess](https://docs.devexpress.com/MobileControls/DevExpress.XamarinForms.DataGrid.CustomSummaryProcess) class with the [DataSummaryProcess](http://docs.devexpress.com/MAUI/DevExpress.Maui.Core.DataSummaryProcess?v=22.2) because of the [T1120574 - DataGridView and DXCollectionView - API Changes](https://supportcenter.devexpress.com/ticket/details/t1120574/datagridview-and-dxcollectionview-api-changes) breaking API change:

```diff
- void grid_CalculateCustomSummary(System.Object sender, DevExpress.XamarinForms.DataGrid.CustomSummaryEventArgs e) {
+ void grid_CalculateCustomSummary(System.Object sender, DevExpress.Maui.DataGrid.CustomSummaryEventArgs e) {
    if (e.FieldName.ToString() == "Shipped")
        if (e.IsTotalSummary) {
-            if (e.SummaryProcess == CustomSummaryProcess.Start) {
+            if (e.SummaryProcess == DataSummaryProcess.Start) {
                count = 0;
            }
-            if (e.SummaryProcess == CustomSummaryProcess.Calculate) {
+            if (e.SummaryProcess == DataSummaryProcess.Calculate) {
                if (!(bool)e.FieldValue)
                    count++;
                e.TotalValue = count;
            }
        }
}
```

### Step 3.4: Move Platform Files to the Platform Folder

In this sample, we moved files from the [DataGrid_GettingStarted.Android](https://github.com/DevExpress-Examples/maui-migrate-grid-control-from-xamarin-forms/pull/1/files#diff-9c065e2f5966c1245f991ee784b6c3a8d676f7a4e4dc86ad6a768471f70d7c7e) and [DataGrid_GettingStarted.iOS](https://github.com/DevExpress-Examples/maui-migrate-grid-control-from-xamarin-forms/pull/1/files#diff-d615ba770b91dcd91eaade572f528297c0edb57fda54751acb7f787b4843e3d6) projects to the [Platforms/Android](https://github.com/DevExpress-Examples/maui-migrate-grid-control-from-xamarin-forms/pull/1/files#diff-fa67dbba651771c09d6053ac3d21e72d3127ac09ee6cf5a95dcb5b343ef72cd4) and [Platforms/iOS](https://github.com/DevExpress-Examples/maui-migrate-grid-control-from-xamarin-forms/pull/1/files#diff-8c2596fb4226998e047eddd92d1b8948ceb590bc4a632a54fec5dccebad5faf3) folder in the new project, respectively. 

In the `DataGrid_GettingStarted.Android` project, we removed all the folders that start with `mipmap`. You can also remove the [Assets.xassets](DataGrid_GettingStarted.iOS/Assets.xcassets/AppIcon.appiconset/Contents.json) folder from the `DataGrid_GettingStarted.iOS` project. Xamarin.Forms projects required these folders, but the .NET MAUI project does not: the .NET MAUI resizes these images automatically.


## The Result Application Structure 

The following figure illustrates the .NET MAUI solution file hierarchy:

![image](https://user-images.githubusercontent.com/12169834/201390885-ca3941db-e1db-4af0-959e-0cb3b5390d81.png)


## Files to Review

- [Project File](https://github.com/DevExpress-Examples/maui-migrate-grid-control-from-xamarin-forms/pull/1/files#diff-0c0026324b1c4e828e8afa24df6ccf414fd9f1c2d2ed7c39f276e8973f510217)
- [MauiProgram.cs](https://github.com/DevExpress-Examples/maui-migrate-grid-control-from-xamarin-forms/pull/1/files#diff-49ce2dc72608f01978cc1298de6967e0cc1584c1dbe9cceb7c853415bfc5d419)
- [MainPage.xaml](https://github.com/DevExpress-Examples/maui-migrate-grid-control-from-xamarin-forms/pull/1/files#diff-46feb52f587f3e1f6faaf4f23c928c93a67eccb05ddc2000db6303dc3f10ff62)
- [MainPage.xaml.cs](https://github.com/DevExpress-Examples/maui-migrate-grid-control-from-xamarin-forms/pull/1/files#diff-6eac6ac4461489f83db3a88bb5d9c2b916cddb8a933bd285d9704b555e5d4375)

## Documentation

- [DevExpress Documentation: Migration from Xamarin.Forms to .NET MAUI](https://docs.devexpress.com/MAUI/403988/)
- [DotNet MAUI Repository: Migrating from Xamarin.Forms (Preview)](https://github.com/dotnet/maui/wiki/Migrating-from-Xamarin.Forms-(Preview))
- [DotNet MAUI Repository: Porting Custom Renderers to Handlers](https://github.com/dotnet/maui/wiki/Porting-Custom-Renderers-to-Handlers)
