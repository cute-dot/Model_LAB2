using System;
using System.Collections.Generic;
using System.Linq;

namespace Model;

public class LineSimulation
{
    private List<WorkStation> workstations; // Список рабочих мест
    private List<Item> delayedProducts; // Список отложенных изделий
    private double currentTime; // Текущее время моделирования
    private Random random;
    
    public LineSimulation()
    {
        // Инициализация рабочих мест
        workstations = new List<WorkStation>
        {
            new() { Queue = new Queue<Item>(), ProcessingTime = 1.25, Id = 0},
            new() { Queue = new Queue<Item>(), ProcessingTime = 0.5, Id = 1}
        };

        delayedProducts = new List<Item>();
        currentTime = 0;
        random = new Random();
    }

    public void Simulate(int numItems)
    {
        for (int i = 0; i < numItems; i++)
        {
            ProcessWorkstation(workstations[0]);
            ProcessWorkstation(workstations[1]);
            // Генерация времени поступления и времени обработки для нового изделия
            var arrivalTime = currentTime + ExponentialRandom(0.4);
            // double processingTime = random.NextDouble() < 0.5 ? 1.25 : 0.5;

            Item product = new Item {Id = i, ArrivalTime = arrivalTime};
            currentTime = arrivalTime;

            bool isDelayed = ProcessProduct(product); // Обработка нового изделия
            if (isDelayed)
            {
                delayedProducts.Add(product);
            }
        }
    }
    private bool ProcessProduct(Item product)
    {
        if (workstations[0].Queue.Count + workstations[1].Queue.Count >= 8)
        {
            return true;
        }
        //
        // if (workstations[1].Queue.Count >= 2)
        // {
        //     workstations[0].IsBlocked = true;
        //     return true;
        // }
        //
        // if (workstations[0].IsBlocked)
        // {
        //     return true;
        // }

        if (workstations[0].Queue.Count() > workstations[1].Queue.Count())
        {
            workstations[1].Queue.Enqueue(product);
        }
        else
        {
            workstations[0].Queue.Enqueue(product);
        }
        return false;
    }
    
    private void ProcessWorkstation(WorkStation workstation)
    {
        if (workstation.Queue.Count > 0)
        {
            Item product = workstation.Queue.Dequeue();
            double processingTime = workstation.ProcessingTime;
            currentTime += processingTime;
            // if (workstation == workstations[0] && workstations[1].Queue.Count < 2)
            // {
            //     ProcessWorkstation(workstations[1]);
            //     workstation.IsBlocked = false;
            // }
            Console.WriteLine(currentTime);
            Console.WriteLine($"Изделие {product.Id} обработано на рабочем месте {workstation.Id} за время {processingTime}");
        }
        
    }
    
    public double ExponentialRandom(double lambda)
    {
        return -Math.Log(1 - random.NextDouble()) / lambda;
    }
}