using UnityEngine;

public class CameraFader : MonoBehaviour
{
    
    public Color fadeColor = Color.black;
    public bool startHidden;
    
    private float _fadeTime;
    private float _currentAlpha;
    private float _toAlpha;
    private Texture2D _texture;
    private int _direction = 1;
    
    public void FadeIn(float time)
    {
        _fadeTime = Mathf.Abs(time);
        _direction = 1;
        _currentAlpha = 0.0f;
        _toAlpha = 1.0f;
    }
    
    public void FadeOut(float time)
    {
        _fadeTime = Mathf.Abs(time);
        _direction = -1;
        _currentAlpha = 1.0f;
        _toAlpha = 0.0f;
    }
    
    
    public void Start()
    {
        _texture = new Texture2D(1, 1);
        _currentAlpha = startHidden ? 1 : 0;
        _texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, _currentAlpha));
        _texture.Apply();
    }
    
    public void OnGUI()
    {

        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _texture);

        bool condition = _direction == 1 ? _currentAlpha < _toAlpha : _currentAlpha > _toAlpha;
        
        if (condition)
        {
            _currentAlpha += _direction * Time.deltaTime / _fadeTime;
            _texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, _currentAlpha));
            _texture.Apply();
        }
        
    }

}