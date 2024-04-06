using System;
using System.Collections.Generic;

namespace Model;

public class WorkStation
{
    public Queue<Item> Queue { get; set; } // Очередь изделий на рабочем месте
    public double ProcessingTime { get; set; } // Время обработки на рабочем месте
    public bool IsBlocked { get; set; } // Флаг блокировки рабочего места
    public int Id { get; set; }
}