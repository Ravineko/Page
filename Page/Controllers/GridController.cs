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

        [HttpPost("CalculateParts")]
        public JsonResult CalculateParts([FromBody] CalculationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { errorMessage = "Invalid request data." });
            }

            bool[,] isCut = new bool[request.Rows, request.Columns];

            foreach (var cell in request.CutCells)
            {
                isCut[cell.Row, cell.Column] = true;
            }

            int partsCount = 0;
            for (int row = 0; row < request.Rows; row++)
            {
                for (int column = 0; column < request.Columns; column++)
                {
                    if (!isCut[row, column])
                    {
                        DFS(isCut, row, column, request.Rows, request.Columns);
                        partsCount++;
                    }
                }
            }

            return Json(new { partsCount });
        }

        private void DFS(bool[,] isCut, int row, int column, int rows, int columns)
        {
            if (row < 0 || row >= rows || column < 0 || column >= columns || isCut[row, column])
            {
                return;
            }

            isCut[row, column] = true;

            int[] dr = { -1, 1, 0, 0 };
            int[] dc = { 0, 0, -1, 1 };
            for (int i = 0; i < 4; i++)
            {
                int newRow = row + dr[i];
                int newColumn = column + dc[i];
                DFS(isCut, newRow, newColumn, rows, columns);
            }
        }
    }

    public class CalculationRequest
    {
        public List<Cell> CutCells { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
    }
}
