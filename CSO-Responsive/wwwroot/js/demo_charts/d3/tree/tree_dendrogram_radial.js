/* ------------------------------------------------------------------------------
 *
 *  # D3.js - radial dendrogram layout
 *
 *  Demo of radial dendrogram layout setup with .json data source
 *
 * ---------------------------------------------------------------------------- */


// Setup module
// ------------------------------

var D3TreeDendrogramRadial = function() {


    //
    // Setup module components
    //

    // Chart
    var _treeDendrogramRadial = function() {
        if (typeof d3 == 'undefined') {
            console.warn('Warning - d3.min.js is not loaded.');
            return;
        }

        // Main variables
        var element = document.getElementById('d3-dendrogram-radial'),
            diameter = 900;


        // Initialize chart only if element exsists in the DOM
        if(element) {

            // Basic setup
            // ------------------------------

            // Define main variables
            var d3Container = d3.select(element);

            // Colors
            var color = '#2196F3';


            // Create chart
            // ------------------------------

            // Add SVG element
            var container = d3Container.append("svg");

            // Add SVG group
            var svg = container
                .attr("width", diameter)
                .attr("height", diameter)
                .append("g")
                    .attr("transform", "translate(" + (diameter / 2) + "," + (diameter / 2) + ")");



            // Construct chart layout
            // ------------------------------

            // Cluster
            var cluster = d3.layout.cluster()
                .size([360, (diameter / 2) - 150]);

            // Diagonal projection
            var diagonal = d3.svg.diagonal.radial()
                .projection(function(d) { return [d.y, d.x / 180 * Math.PI]; });


            // Load data
            // ------------------------------

            d3.json("../../../../demo_data/d3/tree/tree_data_dendrogram_radial.json", function(error, root) {

                var nodes = cluster.nodes(root);


                // Links
                // ------------------------------

                // Append link paths
                var link = svg.selectAll(".d3-tree-link")
                    .data(cluster.links(nodes))
                    .enter()
                    .append("path")
                        .attr("class", "d3-tree-link d3-line-connect")
                        .attr("d", diagonal)
                        .style("stroke-width", 1.5);


                // Nodes
                // ------------------------------

                // Append node group
                var node = svg.selectAll(".d3-tree-node")
                    .data(nodes)
                    .enter()
                    .append("g")
                        .attr("class", "d3-tree-node")
                        .attr("transform", function(d) { return "rotate(" + (d.x - 90) + ")translate(" + d.y + ")"; })

                // Append circles
                node.append("circle")
                    .attr("r", 4.5)
                    .attr("class", "d3-line-circle")
                    .style("stroke", color)
                    .style("stroke-width", 1.5);

                // Append text
                node.append("text")
                    .attr("class", "d3-text")
                    .attr("dy", ".31em")
                    .attr("text-anchor", function(d) { return d.x < 180 ? "start" : "end"; })
                    .attr("transform", function(d) { return d.x < 180 ? "translate(8)" : "rotate(180)translate(-8)"; })
                    .style("font-size", 12)
                    .text(function(d) { return d.name; });
            });
        }
    };


    //
    // Return objects assigned to module
    //

    return {
        init: function() {
            _treeDendrogramRadial();
        }
    }
}();


// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function() {
    D3TreeDendrogramRadial.init();
});
