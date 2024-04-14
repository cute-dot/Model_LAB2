using System;
using System.Collections.Generic;
using System.Linq;

namespace Model;

public class LineSimulation
{
    private List<WorkStation> workstations; // Список рабочих мест
    private List<Item> delayedProducts;
    private List<Item> delayedProducts2;// Список отложенных изделий
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
        delayedProducts2 = new List<Item>();
        currentTime = 0;
        random = new Random();
    }

    public void Simulate(int numItems)
    {
        for (int i = 0; i < numItems; i++)
        {
            
            // Генерация времени поступления и времени обработки для нового изделия
            var delayTime = ExponentialRandom(0.4);
            var arrivalTime = currentTime + delayTime;
            // double processingTime = random.NextDouble() < 0.5 ? 1.25 : 0.5;
            
            Item product = new Item {Id = i, ArrivalTime = arrivalTime};

            ProcessProductForFirstWorkStation(product, delayTime);
            
            ProcessWorkstation(workstations[0]);
            ProcessWorkstation(workstations[1]);
            // }
        }

        Console.WriteLine(delayedProducts.Count);
    }
    private void ProcessProductForFirstWorkStation(Item product, double delayTime)
    {
        if (workstations[0].IsBlocked == false)
        {
            if (workstations[0].Queue.Count <= 4)
            {
                workstations[0].Queue.Enqueue(product);
            }
            if (ExponentialRandom(workstations[0].ProcessingTime) > delayTime)
            {
                delayedProducts.Add(product);
                Console.WriteLine("product v delay");
            }
        }
        else
        {
            delayedProducts.Add(product);
            Console.WriteLine("product v delay");
        }
    }
    private void ProcessProductForSecWorkStation(Item product)
    {
        if (workstations[1].Queue.Count <= 2)
        {
            workstations[1].Queue.Enqueue(product);
            
        }
        else
        {
            workstations[0].IsBlocked = true;
        }
        
    }
    private void ProcessWorkstation(WorkStation workstation)
    {
        if (workstation == workstations[0] && workstation.Queue.Count() > 0 && workstation.IsBlocked != true)
        {
            Item product = workstation.Queue.Dequeue();
            double processingTime = workstation.ProcessingTime;
            currentTime += ExponentialRandom(processingTime);
            if (workstation == workstations[0])
            {
                ProcessProductForSecWorkStation(product);
            }
            Console.WriteLine(ExponentialRandom(processingTime));
            Console.WriteLine($"Текущее время обработки всех элементов {currentTime}");
            Console.WriteLine($"Изделие {product.Id} обработано на рабочем месте {workstation.Id} за время {processingTime}");
        }
        if(workstation == workstations[1] && workstations[1].Queue.Count > 0)
        {
            Item product = workstation.Queue.Dequeue();
            double processingTime = workstation.ProcessingTime;
            currentTime += ExponentialRandom(processingTime);
            workstations[0].IsBlocked = false;
            Console.WriteLine(ExponentialRandom(processingTime));
            Console.WriteLine($"Текущее время обработки всех элементов {currentTime}");
            Console.WriteLine($"Изделие {product.Id} обработано на рабочем месте {workstation.Id} за время {processingTime}");
        }
    }
    
    public double ExponentialRandom(double lambda)
    {
        return -Math.Log(1 - random.NextDouble()) / lambda;
    }
}