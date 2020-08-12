using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class Agent : MonoBehaviour
//{
//    public Vector2 destination;
//    Vector2 target_orientation;
//    float speed = 0.5f;
//    float change_in_angle;
//    float rotSpeed = 30f;
//    float backward_movement_modifier = 0.7f; // moving backwards is 70% slower
//    public bool can_move = true;
//    public bool can_rotate = true;
//    public int max_health = 50;
//    public int current_health;
//    //public HealthBar health_bar;
//    public static Plane arenaPlane = new Plane(Vector3.up, Vector3.zero);
//
//
//
//
//    //List<Ability> abilities = new List<Ability>();
//
//
//    //void createHealthBar() {
//    //    GameObject prefab = Resources.Load("HealthBarCanvas") as GameObject;
//    //    GameObject health_bar_obj = Instantiate(prefab, transform.position, transform.rotation, transform);
//    //    health_bar_obj.transform.localPosition = new Vector3(0,0.5f,0);
//    //    health_bar = health_bar_obj.GetComponent<HealthBar>();
//    //    health_bar.setMaxHealth(max_health);
//    //    current_health = max_health;
//    //    health_bar.setHealth(current_health);
//
//    //}
//
//
//
//    // Start is called before the first frame update
//    void Start()
//    {
//        //createHealthBar();
//        destination = new Vector2(0f, 0f);
//        change_in_angle = 0;
//        //abilities.Add(new DirectedDash(this, 90f, 2f, 8f, 0.5f, KeyCode.F));
//        //abilities.Add(new DirectedDash(this, -90f, 2f, 8f, 0.5f, KeyCode.A));
//        //abilities.Add(new DirectedDash(this, 0f, 2f, 8f, 0.5f, KeyCode.S));
//        //abilities.Add(new DirectedDash(this, 180f, 2f, 8f, 0.5f, KeyCode.D));
//        //abilities.Add(new Sword(this, 1f, KeyCode.R));
//    }
//
//   // void takeDamage(int damage) {
//   //   current_health -= damage;
//   //   health_bar.setHealth(current_health);
//   // }
//
//    public Vector2 OrientationVector() {
//        float angle = transform.eulerAngles.y;
//        Vector2 current_orientation = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), -1f * Mathf.Sin(Mathf.Deg2Rad * angle));
//        current_orientation.Normalize();
//        return current_orientation;
//    }
//
//    public bool atDestination() {
//      Debug.Log(this.transform.localPosition);
//      Debug.Log(this.destination);
//      return Utility.v3to2(this.transform.localPosition) == this.destination;
//    }
//
//
//    // Update is called once per frame
//    void Update()
//    {
//        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//        float ray_enter = 0f;
//        arenaPlane.Raycast(ray, out ray_enter);
//        Vector2 plane_position = new Vector2(transform.localPosition.x, transform.localPosition.z);
//        Vector2 target = Utility.v3to2(ray.GetPoint(ray_enter));
//        if (Input.GetMouseButtonDown(1))
//        {
//            destination = target;
//        }
//
//        if (Input.GetMouseButton(0))
//        {
//            //takeDamage(5);
//            target_orientation = target - plane_position;
//
//        }
//
//        Vector2 current_orientation = OrientationVector();
//        change_in_angle = Vector2.SignedAngle(target_orientation, current_orientation);
//        change_in_angle = Mathf.Clamp(change_in_angle, -rotSpeed, rotSpeed);
//
//
//        //Debug.Log("change in angle: " + change_in_angle);
//        //Debug.Log("current_orientation: " + current_orientation);
//        
//        Vector3 displacement = destination - plane_position;
//
//
//        displacement = Vector2.ClampMagnitude(displacement, speed);
//
//        // adjust movement speed according to object orientation.
//        // If moving backward, move at half speed. Intermediate angles interpolated linearly.
//        if (displacement.magnitude > 0) {
//          float object_angle_to_movement = Vector2.Angle(current_orientation, displacement);
//          float movement_modifier = 1f - backward_movement_modifier * object_angle_to_movement  / 180f ;
//          displacement *= movement_modifier;
//        }
//          
//        if (can_move) {
//          transform.localPosition += new Vector3(displacement.x, 0f, displacement.y);
//        }
//        
//        if (can_rotate) {
//          transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y + change_in_angle, 0f);
//        }
//
//
//        // process all abilitites
//        //foreach (Ability ability in abilities) {
//        //  ability.update();
//        //}
//    }
//}
//
