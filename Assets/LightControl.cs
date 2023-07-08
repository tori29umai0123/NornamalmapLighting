using UnityEngine;
using UnityEngine.UI;

public class LightControl : MonoBehaviour
{
    public GameObject Directional_Light;
    public GameObject Point_Light;

    public Slider Directional_slider_power;
    public Slider Directional_slider_X;
    public Slider Directional_slider_Y;
    public Slider Point_Light_slider_power;
    public Slider Point_Light_slider_range;
    public Slider Point_slider_X;
    public Slider Point_slider_Y;

    public void Directional_Light_power()
    {
        var value = Directional_slider_power.value;
        Directional_Light.GetComponent<Light>().intensity = value;
    }

    public void RotateObject_X()
    {
        var valueX = Directional_slider_X.value;
        var valueY = Directional_slider_Y.value;
        // Rotationを変更する
        Directional_Light.transform.rotation = Quaternion.Euler(valueX, valueY, 0f);
    }

    public void RotateObject_Y()
    {
        var valueX = Directional_slider_X.value;
        var valueY = Directional_slider_Y.value;
        // Rotationを変更する
        Directional_Light.transform.rotation = Quaternion.Euler(valueX, valueY, 0f);
    }

    public void Point_Light_power()
    {
        var value = Point_Light_slider_power.value;
        Point_Light.GetComponent<Light>().intensity = value;
    }

    public void Point_Light_range()
    {
        var value = Point_Light_slider_range.value;
        Point_Light.GetComponent<Light>().range = value;
    }
    public void PositionObject_X()
    {
        var value = Point_slider_X.value;
        // Positionを変更する
        Vector3 newPosition = new Vector3(value, Point_Light.transform.position.y, Point_Light.transform.position.z);
        Point_Light.transform.position = newPosition;
    }

    public void PositionObject_Y()
    {
        var value = Point_slider_Y.value;
        // Positionを変更する
        Vector3 newPosition = new Vector3(Point_Light.transform.position.x, value, Point_Light.transform.position.z);
        Point_Light.transform.position = newPosition;
    }



}
