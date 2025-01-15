using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    [SerializeField] private AudioSource ObjSource;
    [SerializeField] private AudioClip pickUpObj, dropObj, objDone;

    private bool IsDragging, objPlaced, freePlacement;
    private Vector2 ObjOffset, origPosition, origScale;
    private Quaternion origRotation;
    private Rigidbody2D objRB;
    private Transform closestSlot, closestFreeSlot, currentSlot;
    private ObjectSlot[] slots;
    private ObjectFreeSlot[] freeslots;
    Vector2 GetTapPos() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    // Ниже запоминаем базовые значения яблока
    void Awake() {
        origPosition = transform.position;
        origRotation = transform.rotation;
        origScale = transform.localScale;
        objRB = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if(objPlaced) return;
        if(!IsDragging) return;

        transform.position = GetTapPos() - ObjOffset;
    }

    void FindClosestSlot() {
        // Получаем все слоты в сцене
        ObjectSlot[] slots = FindObjectsByType<ObjectSlot>(FindObjectsSortMode.InstanceID);

        float minDistance = Mathf.Infinity;
        foreach (ObjectSlot objSlot in slots)
        {
            float distance = Vector2.Distance(GetTapPos(), objSlot.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestSlot = objSlot.transform;
            }
        }
    }

    void FindClosestFreeSlot() {
        // Получаем все фрислоты в сцене
        ObjectFreeSlot[] freeslots = FindObjectsByType<ObjectFreeSlot>(FindObjectsSortMode.InstanceID);

        float minDistance = Mathf.Infinity;
        foreach (ObjectFreeSlot objFreeSlot in freeslots)
        {
            float distance = Vector2.Distance(GetTapPos(), objFreeSlot.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestFreeSlot = objFreeSlot.transform;
            }
        }
    }

        // Находим свободное место на полке и меняем размер яблока
        // Размер меняется в зависимости от дистанции нижней грани яблока до нижней грани полки
        // Чем дальше грань яблока от нижней грани полки тем яблоко меньше
        // Размеры клэмпнуты (ограничены)
    void FindFreeSpace() {
        FindClosestFreeSlot();
        Collider2D freeSlotBS = closestFreeSlot.GetComponent<Collider2D>();
        Collider2D appleBS = GetComponent<Collider2D>();
        Vector2 bottomAppleBS = new Vector2(appleBS.bounds.center.x, appleBS.bounds.min.y);
        float appleBtPos = transform.position.y - transform.localScale.y;
        float freeSlotPos = closestFreeSlot.position.y + closestFreeSlot.localScale.y;
        float curDistance = appleBtPos - freeSlotPos;
        if(!freeSlotBS.OverlapPoint(bottomAppleBS)){
            currentSlot = closestSlot;
            freePlacement = false;
        }else{
            currentSlot = closestFreeSlot;
            freePlacement = true;
            transform.position = new Vector2(transform.position.x,transform.position.y - transform.localScale.y * 6);
            float sizeFactor = Mathf.Clamp01(Mathf.Abs(curDistance) / (freeSlotPos - (freeSlotPos - transform.localScale.y * 10)));
            transform.localScale = origScale * Mathf.Lerp(0.8f, 0.5f, sizeFactor);
        }
    }

        // Нажатие
    void OnMouseDown() {
        transform.localScale = origScale;
        objRB.linearVelocity = Vector2.zero;
        objRB.angularVelocity = 0;
        objPlaced = false;
        IsDragging = true;
        transform.rotation = origRotation;
        objRB.bodyType = RigidbodyType2D.Kinematic;
        ObjSource.PlayOneShot(pickUpObj);
        ObjOffset = GetTapPos() - (Vector2)transform.position;
    }

        // Отпускаем
        // Тут проверки на ближайшие слоты, чтобы добавлять их бесконечно много без изменения кода
        // Если яблоко находится в границах свободных слотов (полок) то преимущество отдается им
        // Если яблоко около фиксированного слота, то оно вставляется и принимает его свойства
        // Если яблоко не находится ни с чем рядом, то оно просто падает с физикой
        // Если яблоко вблизи у стартовой позиции, оно ставится как будто в слот на спавн
    void OnMouseUp() {
            FindClosestSlot();
            FindFreeSpace();
            objPlaced = false;
            IsDragging = false;
            objRB.bodyType = RigidbodyType2D.Dynamic;
            if(currentSlot != null && (Vector2.Distance(transform.position, currentSlot.transform.position) < 1.5)){
                objRB.bodyType = RigidbodyType2D.Kinematic;
                objPlaced = true;
                ObjSource.PlayOneShot(objDone);
                if(!freePlacement){
                    transform.position = currentSlot.transform.position;
                    transform.localScale = currentSlot.transform.localScale;
                }
                transform.rotation = origRotation;
            }else{
                ObjSource.PlayOneShot(dropObj);
                if(Vector2.Distance(transform.position, origPosition) < 1){
                    objRB.bodyType = RigidbodyType2D.Kinematic;
                    transform.position = origPosition;
                    transform.rotation = origRotation;
                }else{
                    IsDragging = false;
                    transform.position = GetTapPos();
                    transform.rotation = origRotation;
                }
            }
    }
}
