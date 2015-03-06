open System
open System.Drawing
open System.Windows.Forms

// Create a form to display the graphics
let width, height = 500, 500
let form = new Form(Width = width, Height = height)
let box = new PictureBox(BackColor = Color.White, Dock = DockStyle.Fill)
let image = new Bitmap(width, height)
let graphics = Graphics.FromImage(image)
//The following line produces higher quality images, 
//at the expense of speed. Uncomment it if you want
//more beautiful images, even if it's slower.
//Thanks to https://twitter.com/AlexKozhemiakin for the tip!
graphics.SmoothingMode <- System.Drawing.Drawing2D.SmoothingMode.HighQuality

box.Image <- image
form.Controls.Add(box)

// Compute the endpoint of a line
// starting at x, y, going at a certain angle
// for a certain length. 
let endpoint x y angle length =
    x + length * cos angle,
    y + length * sin angle

let flip x = (float)height - x

// Utility function: draw a line of given width, 
// starting from x, y
// going at a certain angle, for a certain length.
let drawLine (target : Graphics) (brush : Brush) 
             (x : float) (y : float) 
             (angle : float) (length : float) (width : float) =
    let x_end, y_end = endpoint x y angle length
    let origin = new PointF((single)x, (single)(y |> flip))
    let destination = new PointF((single)x_end, (single)(y_end |> flip))
    let pen = new Pen(brush, (single)width)
    target.DrawLine(pen, origin, destination)

let draw x y angle length width = 
    let red = max 10 (if length > 100. then int length - 50 else int length + 50)
    let brush = new SolidBrush(Color.FromArgb(red, 100, 50))
    drawLine graphics brush x y angle length width

let pi = Math.PI

// Now... your turn to draw
// The trunk
//draw 250. 50. (pi*(0.5)) 100. 4.
//let x, y = endpoint 250. 50. (pi*(0.5)) 100.
// first and second branches
//draw x y (pi*(0.5 + 0.3)) 50. 2.
//draw x y (pi*(0.5 - 0.4)) 50. 2.

let drawTree x y angle length width =
    let rec drawBranch x y angle length width div =
        if div > 1. then
            draw x y (pi*angle) length width
            let x', y' = endpoint x y (pi*angle) length
            let length', width', div' = length/div, width/div, div*0.95
            drawBranch x' y' (angle + (0.2*0.82)) length' width' div'
            drawBranch x' y' (angle - (0.3*0.78)) length' width' div'
        else ()
    drawBranch x y angle length width 2.
drawTree 250. 50. 0.5 200. 5.

form.ShowDialog()

(* To do a nice fractal tree, using recursion is
probably a good idea. The following link might
come in handy if you have never used recursion in F#:
http://en.wikibooks.org/wiki/F_Sharp_Programming/Recursion
*)