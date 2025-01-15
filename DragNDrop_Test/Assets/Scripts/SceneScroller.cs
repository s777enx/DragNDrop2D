using UnityEngine;

public class SceneScroller : MonoBehaviour
{
    public float scrollSpeed = 8f;
    public GameObject background;
    private Vector3 targetPosition;
    private float minX;
    private float maxX;
    private float screenWidth;

    void Start()
    {
        targetPosition = transform.position;

        // Размеры фона с помощью спрайтера
        SpriteRenderer bgRenderer = background.GetComponent<SpriteRenderer>();

        // Ширина экрана через размер камеры и соотношение сторон
        screenWidth = Camera.main.orthographicSize * Camera.main.aspect;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;

        float cameraLeftEdge = Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect;
        float bgLeftEdge = background.GetComponent<Renderer>().bounds.min.x;
        float cameraRightEdge = Camera.main.transform.position.x + Camera.main.orthographicSize * Camera.main.aspect;
        float bgRightEdge = background.GetComponent<Renderer>().bounds.max.x;

            // Ширина экрана в пикселях
            float pixelScreenWidth = Screen.width;

            if (mousePosition.x < pixelScreenWidth / 8) // Листаем влево конечную точку
            {
                if(cameraLeftEdge - 3 >= bgLeftEdge){
                    targetPosition += new Vector3(-3, 0, 0);
                }else{
                    targetPosition = new Vector3(bgLeftEdge + Camera.main.orthographicSize * Camera.main.aspect, Camera.main.transform.position.y, Camera.main.transform.position.z);
                } // Проверяем можно ли ехать влево, соотносим левый край фона с левым краем видимости камеры
            }
            else if (mousePosition.x > pixelScreenWidth * 0.9) // Листаем вправо конечную точку
            {
                if(cameraRightEdge + 3 <= bgRightEdge){ 
                targetPosition += new Vector3(3, 0, 0);
                }else{
                    targetPosition = new Vector3(bgRightEdge - Camera.main.orthographicSize * Camera.main.aspect, Camera.main.transform.position.y, Camera.main.transform.position.z);
                } // Проверяем можно ли ехать вправо, соотносим правый край фона с левым краем видимости камеры
            }
        }

        // Передвижение камеры в конечную точку с заданной скоростью
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, scrollSpeed * Time.deltaTime);
    }
}