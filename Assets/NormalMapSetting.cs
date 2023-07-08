using System;
using UnityEngine;
using SFB;
using UnityEngine.UI;
using System.IO;

public class NormalMapSetting : MonoBehaviour
{
    // 変数の定義
    public string path;                          // 選択したファイルのパスを格納する変数
    public Material material;                    // マテリアルを格納する変数
    public RawImage rawImage;                     // RawImageコンポーネントを格納する変数
    public Camera ImageCamera;                     // RawImageコンポーネントを格納する変数
    public RenderTexture CameraTexture;           // レンダーテクスチャを格納する変数
    public GameObject Plane;                      // Planeオブジェクトを格納する変数
    Texture2D normalMapTexture;

    // 画像のロードを行うメソッド
    public void LoadImage()
    {
        // ファイルのフィルターを設定
        var extensions = new[]
        {
        new ExtensionFilter("PNG Files", "png"),
        new ExtensionFilter("JPEG Files", "jpg", "jpeg")
        };

        // ファイルパネルを表示してファイルの選択を待つ
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Open Normal Map", "", extensions, false);


        // Pathは文字列の配列だから、1つの場合は1個のみかの判定 + null判定
        if (paths.Length == 1 && paths[0] != null)
        {
            path = paths[0];

            // 選択されたテクスチャをロードし、正規化する
            normalMapTexture = LoadTexture(path);

            // マテリアルにロードしたテクスチャをセットする
            SetNormalMap(normalMapTexture);

            // Panelが正方形なので正方形で読み込む
            RenderTexture rt = new RenderTexture(normalMapTexture.width, normalMapTexture.width, 24);
            RenderTexture.active = rt;
            CameraTexture = rt;

            ImageCamera.targetTexture = CameraTexture;

            // レンダーテクスチャをRAWImageに設定する
            rawImage.texture = ImageCamera.targetTexture;

            // RAWImageのサイズを調整する
            AdjustRawImageSize(normalMapTexture.width, normalMapTexture.height);
        }

    }


    // 画像を保存するメソッド
    public void SaveImage()
    {
        // レンダーテクスチャをテクスチャ2Dに変換し、PNG形式で保存する
        Texture2D texture = new Texture2D(CameraTexture.width, CameraTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = CameraTexture;
        texture.ReadPixels(new Rect(0, 0, CameraTexture.width, CameraTexture.height), 0, 0);
        texture.Apply();
        RenderTexture.active = null;

        texture = ResizeTexture(texture, normalMapTexture.width, normalMapTexture.height);

        byte[] imageData = texture.EncodeToPNG();

        // 保存先のパスを指定し、画像を保存する
        string savePath = StandaloneFileBrowser.SaveFilePanel("Save Image", "", "image.png", "png");
        if (string.IsNullOrEmpty(savePath))
            return;

        File.WriteAllBytes(savePath, imageData);
    }

    private Texture2D ResizeTexture(Texture2D texture, int width, int height)
    {
        RenderTexture rt = new RenderTexture(width, height, 0);
        rt.Create();

        Graphics.Blit(texture, rt);

        RenderTexture.active = rt;
        Texture2D resizedTexture = new Texture2D(width, height);
        resizedTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        resizedTexture.Apply();
        RenderTexture.active = null;

        return resizedTexture;

    }




    // RAWImageのサイズを調整するメソッド
    private void AdjustRawImageSize(int width, int height)
    {
        const int maxDimension = 600;

        float aspectRatio = (float)width / (float)height;
        float targetWidth = width;
        float targetHeight = height;

        if (width > maxDimension || height > maxDimension)
        {
            if (aspectRatio > 1)
            {
                targetWidth = maxDimension;
                targetHeight = maxDimension / aspectRatio;
            }
            else
            {
                targetHeight = maxDimension;
                targetWidth = maxDimension * aspectRatio;
            }
        }

        rawImage.rectTransform.sizeDelta = new Vector2(targetWidth, targetHeight);
    }


    // 指定されたパスのテクスチャをロードするメソッド
    private Texture2D LoadTexture(string path)
    {
        byte[] imageData = System.IO.File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageData);

        return texture;
    }

    // マテリアルの法線マップテクスチャを設定するメソッド
    private void SetNormalMap(Texture2D normalMapTexture)
    {
        material.SetTexture("_NormalMap", normalMapTexture);
        material.EnableKeyword("_NORMALMAP"); // 法線マップを有効にするキーワードを設定する

    }

    // テクスチャからスプライトを作成するメソッド
    private Sprite CreateSprite(Texture2D texture)
    {
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

        return sprite;
    }



}
