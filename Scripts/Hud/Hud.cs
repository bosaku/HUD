using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Hud : MonoBehaviour
{
    private List<HudSymbol> _hudSymbols = new List<HudSymbol>();
    private ParticleSystem _pSystem;
    private ParticleSystem.Particle[] _particles;
    private ParticleSystem.MainModule _main;
    private ParticleSystem.EmitParams emit;
    public bool update = true;
    private float distanceMult = .02f;

    private void Start()
    {
        _pSystem = FindObjectOfType<ParticleSystem>();

        _main = _pSystem.main;
        _main.maxParticles = 2000;
        _main.startSpeed = 0;
        _main.simulationSpace = ParticleSystemSimulationSpace.World;
        _main.playOnAwake = false;
        _main.loop = false;
    }

    public void Bind(HudSymbol hudSymbol)
    {
        _hudSymbols.Add(hudSymbol);

        emit.position = hudSymbol.transform.position;
        emit.startSize = 1;
        emit.velocity = Vector3.zero;
        emit.startLifetime = float.MaxValue;
        emit.angularVelocity = 0;

        if (!_pSystem) _pSystem = GetComponent<ParticleSystem>();
        _pSystem.Emit(emit, _hudSymbols.Count);
        _pSystem.Play();
        var e = _pSystem.emission;
        e.enabled = true;
    }
    
    private void LateUpdate()
    {
        if (!update) return;

        var displayThese = new List<KeyValuePair<float,HudSymbol>>();
        
        foreach (var symbol in _hudSymbols)
        {
            var d = Vector3.Distance(symbol.transform.position, transform.position);
            
            if (d > 10 && d < 500 && symbol.displaySymbol)
            {
                displayThese.Add( new KeyValuePair<float, HudSymbol>(d,symbol));
            }
            
        }
        
        _main.maxParticles = displayThese.Count;
        _pSystem.GetParticles(_particles = new ParticleSystem.Particle[displayThese.Count]);

        for (int j = 0; j < _particles.Length; j++)
        {
            _particles[j].position = new Vector3(0,-1000,0);
        }
        
        int i = 0;
        foreach (var symbol in displayThese)
        {
            
            _particles[i].position = symbol.Value.transform.position;
            _particles[i].startSize = symbol.Key * distanceMult;
            i++; 
        }

        // int i = 0;
        // foreach (var symbol in _hudSymbols)
        // {
        //     if (!symbol.displaySymbol) continue;
        //     
        //     var d = Vector3.Distance(symbol.transform.position, transform.position);
        //
        //     if (d < 10 || d > 1000)
        //     {
        //         _particles[i].startSize = 0;
        //         i++;
        //         continue;
        //     }
        //         
        //     
        //     _particles[i].position = symbol.transform.position;
        //     _particles[i].startSize = d * .015f;
        //     i++;
        // }

        _pSystem.SetParticles(_particles, _main.maxParticles);
        _pSystem.Play();
        var e = _pSystem.emission;
        e.enabled = true;

        update = false;  
    }

    private void FixedUpdate()
    {
        update = true;
    }
}