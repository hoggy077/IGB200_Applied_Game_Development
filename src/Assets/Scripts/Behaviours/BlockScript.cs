using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumsAndDictionaries;
using static SoundManager;
public class BlockScript : MonoBehaviour, iPhysicsInterface, iReloadInterface
{
    private List<Rigidbody2D> collidedObjects = new List<Rigidbody2D>();
    public float Deceleration;
    public float VelocityOfOnePush;
    public float Deceleration_ { get => Deceleration; set => Deceleration = value; }
    public Rigidbody2D RB_ { get => GetComponent<Rigidbody2D>(); }

    public bool isException_ => false;

    private void Start() {
        GetComponent<SpriteRenderer>().sortingLayerName = "Objects";
    }
    private void FixedUpdate() {
        Decelerate();
        UpdateSorting();
    }

    public void Decelerate() {
        if (RB_.velocity == Vector2.zero) {
            return;
        }

        RB_.velocity -= Deceleration * Time.deltaTime * RB_.velocity;

        if (RB_.velocity.magnitude < 0.25f) {
            RB_.velocity *= 0f;
        }
    }

    public void UpdateVelocity(float magnitude, Vector3 direction){
        magnitude = Mathf.Sign(magnitude) * VelocityOfOnePush;
        RB_.velocity = magnitude * direction;
        Debug.Log(magnitude+ " " + direction);
    }
    public void UpdateForce(float magnitude, Vector3 direction, Elements elementType) {
        if (elementType == Elements.Pull) {
            magnitude *= -1f;
        }
        UpdateVelocity(magnitude, direction);
        Instantiate(SoundDict[elementType.ToString() + "Damage"]);
    }
    private Vector3 NormaliseVector(Vector3 vector) {
        float x = Mathf.Abs(vector.x) / vector.x;
        float y = Mathf.Abs(vector.y) / vector.y;
        float z = Mathf.Abs(vector.z) / vector.z;
        return new Vector3(x, y, z);
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        transform.TryGetComponent(out Rigidbody2D rb);
        bool b1 = collision.transform.TryGetComponent(out BlockScript _);
        bool b2 = collision.transform.TryGetComponent(out Rigidbody2D otherRB);
        if (b1 && b2) {
            collidedObjects.Add(otherRB);
            rb.isKinematic = true;
            rb.useFullKinematicContacts = true;
            rb.velocity = Vector2.zero;
            Vector3 posDifference = transform.position - collision.transform.position;
            rb.MovePosition(transform.position - (posDifference * 0.05f));
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        transform.TryGetComponent(out Rigidbody2D rb);
        if (collision.transform.TryGetComponent(out Rigidbody2D otherRB)) {
            if (collidedObjects.Contains(otherRB)) {
                collidedObjects.Remove(otherRB);
                if(collidedObjects.Count == 0) {
                    rb.isKinematic = false;
                }
            }
        }
    }

    public void UpdateSorting() {
        GetComponent<Renderer>().sortingOrder = -Mathf.RoundToInt(transform.position.y);
    }

}
