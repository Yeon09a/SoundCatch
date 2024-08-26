using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetMaterial : MonoBehaviour
{
    public Material outline;
    
    public List<Material> materials = new List<Material>();
    public Renderer rdr;

    public Vector3 oriPos;
    public Vector3 selectedPos;
    
    public void OnOutline()
    {
        // ���� ���� Ȱ��ȭ�� ���� ���� �۵��ϵ��� ó��
        if (SceneManager.GetActiveScene().name == gameObject.scene.name)
        {
            materials.Add(outline);
            rdr.sharedMaterials = materials.ToArray();
            transform.position = selectedPos;
        }
    }

    public void OffOutline()
    {
        // ���� ���� Ȱ��ȭ�� ���� ���� �۵��ϵ��� ó��
        if (SceneManager.GetActiveScene().name == gameObject.scene.name)
        {
            if (materials.Count == 2)
            {
                materials.RemoveAt(1);
            }
            rdr.sharedMaterials = materials.ToArray();
            transform.position = oriPos;
        }
    }
}
