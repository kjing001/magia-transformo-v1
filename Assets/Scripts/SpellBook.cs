using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using EasyWiFi.Core;

using OpenCVForUnity;
using EasyWiFi.ServerBackchannels;
public class SpellBook{

    public int x;
    public int y;
    public List<int> xs = new List<int>();
    public List<int> ys = new List<int>();
    public int xm; // smoothed 
    public int ym;
    // int Hmin, Hmax, Smin, Smax, Vmin, Vmax;

    public Scalar hsv_max;
    public Scalar hsv_min;
    public Scalar hsv1_min;
    public Scalar hsv1_max;
    public string color;

    public bool isTracked = false;
    public bool isTrackedm = false;

    public SpellBook(string _color)
    {
        x = -1; y = -1;
        color = _color;
        if (_color == "red")
        {
            hsv_max = new Scalar(20 / 2, 100 * 2.55f, 100 * 2.55f);
            hsv_min = new Scalar(0 / 2, 60 * 2.55f, 60 * 2.55f);

            hsv1_max = new Scalar(360 / 2, 100 * 2.55f, 100 * 2.55f);
            hsv1_min = new Scalar(330 / 2, 60 * 2.55f, 60 * 2.55f);
        }
        if (_color == "green")
        {
            hsv_max = new Scalar(200 / 2, 100 * 2.55f, 100 * 2.55f);
            hsv_min = new Scalar(120 / 2, 60 * 2.55f, 60 * 2.55f);
        }
        if (_color == "yellow")
        {
            hsv_max = new Scalar(358 / 2, 2 * 2.55f, 100 * 2.55f);
            hsv_min = new Scalar(0 / 2, 0 * 2.55f, 97 * 2.55f);
        }
    }
    public void setColor(string _color) 
    {
        color = _color;
        if (_color == "red")
        {
            hsv_max = new Scalar(30 /2, 100 *2.55f, 100 * 2.55f);
            hsv_min = new Scalar(0 / 2, 50 * 2.55f, 50 * 2.55f);

            hsv1_max = new Scalar(358 / 2, 100 * 2.55f, 100 * 2.55f);
            hsv1_min = new Scalar(330 / 2, 50 * 2.55f, 50 * 2.55f);
        }
        if (_color == "green")
        {
            hsv_max = new Scalar(160 / 2, 100 * 2.55f, 100 * 2.55f);
            hsv_min = new Scalar(120 / 2, 45 * 2.55f, 50 * 2.55f);
        }
        if (_color == "yellow") // actually, yellow screen appears white in the camera
        {
            hsv_max = new Scalar(358 / 2, 3 * 2.55f, 100 * 2.55f);
            hsv_min = new Scalar(0 / 2, 0 * 2.55f, 97 * 2.55f);
        }

    }


}
