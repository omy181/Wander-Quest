using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Holylib.Utilities
{

    public class TextureCacher : Singleton<TextureCacher>
    {
        private Dictionary<string, Texture2D> _cachedTextures = new();

        private string _path;

        protected override void Awake()
        {
            base.Awake();

            _path = Path.Combine(Application.persistentDataPath, "Textures");
        }

        public Texture2D GetTextureByID(string id)
        {
            if (_cachedTextures.TryGetValue(id, out Texture2D cachedTexture))
            {
                return cachedTexture;
            }

            cachedTexture = _loadTextureFromDisk(id);

            if (cachedTexture)
            {
                _cachedTextures[id] = cachedTexture;
                return cachedTexture;
            }

            return null;
        }

        public void SaveTexture(string id,Texture2D texture)
        {
            _cachedTextures[id] = texture;

            Directory.CreateDirectory(_path);

            string filePath = Path.Combine(_path, $"{id}.png");
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(filePath, bytes);

            Debug.Log($"Texture saved at: {filePath}");
        }

        private Texture2D _loadTextureFromDisk(string id)
        {
            string filePath = Path.Combine(_path, $"{id}.png");

            if (File.Exists(filePath))
            {
                byte[] bytes = File.ReadAllBytes(filePath);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(bytes);

                return texture;
            }

            return null;
        }
    }
}
