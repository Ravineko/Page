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

        [HttpGet("CalculateParts")]
        public JsonResult CalculateParts(int rows, int columns, [FromQuery] Dictionary<string, int>[] CutCells)
        {
            List<Cell> cutCellsList = new List<Cell>();

            foreach (var cellDict in CutCells)
            {
                if (cellDict.ContainsKey("row") && cellDict.ContainsKey("column"))
                {
                    int row = cellDict["row"];
                    int column = cellDict["column"];
                    cutCellsList.Add(new Cell { Row = row, Column = column });
                }
            }

            bool[,] isCut = new bool[rows, columns];
            foreach (var cell in cutCellsList)
            {
                isCut[cell.Row, cell.Column] = true;
            }
            int partsCount = 0;
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    if (!isCut[row, column])
                    {
                        DFS(isCut, row, column, rows, columns);
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
}
