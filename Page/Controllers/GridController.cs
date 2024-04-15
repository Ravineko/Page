using Microsoft.AspNetCore.Mvc;
using Page.Models;
using System.Collections.Generic;

namespace Page.Controllers
{
    public class GridController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        /*        [HttpPost("CalculateParts")]
                public JsonResult CalculateParts([FromBody] CalculationRequest request)
                {
                    if (!ModelState.IsValid)
                    {
                        return Json(new { errorMessage = "Invalid request data." });
                    }

                    bool[,] isCut = new bool[request.Rows, request.Columns];

                    List<int> partSizes = new List<int>();
                    for (int row = 0; row < request.Rows; row++)
                    {
                        for (int column = 0; column < request.Columns; column++)
                        {
                            if (!isCut[row, column])
                            {
                                int size = DFS(isCut, row, column, request.Rows, request.Columns);
                                partSizes.Add(size);
                            }
                        }
                    }

                    return Json(new { partsCount = partSizes.Count, partSizes });
                }*/
        [HttpPost("CalculateParts")]
        public JsonResult CalculateParts([FromBody] CalculationRequest request)
        {
            int[,] grid = new int[request.Rows, request.Columns];

            foreach (var cell in request.CutCells)
            {
                int row = cell.Row;
                int col = cell.Column;
                grid[row, col] = 1;
            }

            List<int> partSizes = new List<int>(); // Список для зберігання розмірів кожної частини

            int partsCount = 0;
            bool[,] visited = new bool[request.Rows, request.Columns];

            for (int i = 0; i < request.Rows; i++)
            {
                for (int j = 0; j < request.Columns; j++)
                {
                    if (grid[i, j] == 0 && !visited[i, j])
                    {
                        partsCount++;
                        int partSize = DFS(grid, visited, i, j, request.Rows, request.Columns); // Отримання розміру частини
                        partSizes.Add(partSize); // Додавання розміру до списку
                    }
                }
            }

            return new JsonResult(new { PartsCount = partsCount, PartSizes = partSizes });
        }

        private static int DFS(int[,] grid, bool[,] visited, int row, int col, int rows, int columns)
        {
            if (row < 0 || row >= rows || col < 0 || col >= columns || grid[row, col] == 1 || visited[row, col])
                return 0;

            visited[row, col] = true;

            int size = 1; // Початковий розмір частини

            size += DFS(grid, visited, row + 1, col, rows, columns);
            size += DFS(grid, visited, row - 1, col, rows, columns);
            size += DFS(grid, visited, row, col + 1, rows, columns);
            size += DFS(grid, visited, row, col - 1, rows, columns);

            return size; // Повернення розміру частини
        }

        /*
                private int DFS(bool[,] isCut, int row, int column, int rows, int columns)
                {
                    if (row < 0 || row >= rows || column < 0 || column >= columns || isCut[row, column])
                    {
                        return 0;
                    }

                    isCut[row, column] = true;
                    int size = 1; 

                    int[] dr = { -1, 1, 0, 0 };
                    int[] dc = { 0, 0, -1, 1 };
                    for (int i = 0; i < 4; i++)
                    {
                        int newRow = row + dr[i];
                        int newColumn = column + dc[i];
                        size += DFS(isCut, newRow, newColumn, rows, columns);
                    }

                    return size;
                }*/
    }

    public class CalculationRequest
    {
        public List<Cell> CutCells { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
    }
}
