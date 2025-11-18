let cy = null;

window.initializeCytoscape = function() {
    // Only initialize if cytoscape library is available and container exists
    if (typeof cytoscape === 'undefined') {
        console.warn('Cytoscape.js library not loaded yet');
        return;
    }
    
    const container = document.getElementById('cy');
    if (!container) {
        console.warn('Cytoscape container not found');
        return;
    }

    cy = cytoscape({
        container: container,
        style: [
            {
                selector: 'node',
                style: {
                    'background-color': '#0288d1',
                    'label': 'data(label)',
                    'color': '#fff',
                    'text-valign': 'center',
                    'text-halign': 'center',
                    'width': '100px',
                    'height': '60px',
                    'shape': 'roundrectangle',
                    'font-size': '14px',
                    'text-wrap': 'wrap',
                    'text-max-width': '90px'
                }
            },
            {
                selector: 'edge',
                style: {
                    'width': 2,
                    'line-color': '#ccc',
                    'target-arrow-color': '#ccc',
                    'target-arrow-shape': 'triangle',
                    'curve-style': 'bezier'
                }
            }
        ],
        layout: {
            name: 'breadthfirst',
            directed: true,
            padding: 10,
            spacingFactor: 1.5
        }
    });
};

window.updateCytoscapeGraph = function(nodes, edges) {
    if (!cy) {
        console.warn('Cytoscape not initialized');
        return;
    }

    // Clear existing elements
    cy.elements().remove();

    // Add nodes
    nodes.forEach(node => {
        cy.add({
            group: 'nodes',
            data: { id: node.id, label: node.label }
        });
    });

    // Add edges
    edges.forEach(edge => {
        cy.add({
            group: 'edges',
            data: { source: edge.source, target: edge.target }
        });
    });

    // Apply layout
    cy.layout({
        name: 'breadthfirst',
        directed: true,
        padding: 10,
        spacingFactor: 1.5,
        avoidOverlap: true
    }).run();

    // Fit to container
    cy.fit();
};
