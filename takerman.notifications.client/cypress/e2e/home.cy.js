/// <reference types="cypress" />

describe('home', () => {
  it('successfully loads', () => {
    cy.visit('/');
    cy.url().should('include', '/');
  });

  it("has elements on the page", () => {
    cy.visit('/');
    cy.get('#filter').should('exist');
    cy.get('#languageSelector').should('exist');
    cy.get('#watchlist').should('exist');
    cy.get('#datesList').should('exist');
    cy.get('footer').should('exist');
    cy.get('#headerNavigation').should('exist');
    cy.get('#newsletterForm').should('exist');
    cy.get('#socialBar').should('exist');
    cy.get('#zsiq_float').should('exist');

  });

  it.only("links has the correct URLs", () => {
    let baseUrl = Cypress.config().baseUrl;
    cy.visit('/');

    if (cy.get("#headerLogin") != null) {
      cy.visit('/login');
      cy.get('input[id=email]').type('tanyo@takerman.net');
      cy.get('input[id=password]').type('Hakerman91!');
      cy.get('button[id=btnSubmit]').click();
    }

    cy.get('#footerPrivacyPolicy').click();
    cy.location('pathname').should('eq', '/privacy-policy');
    cy.go('back');

    cy.get('#footerTerms').click();
    cy.location('pathname').should('eq', '/terms-and-conditions');
    cy.go('back');

    cy.get('#footerContacts').click();
    cy.location('pathname').should('eq', '/contacts');
    cy.go('back');

    cy.get('#footerProfile').click();
    cy.location('pathname').should('eq', '/profile');
    cy.go('back');

    cy.get('#footerMatches').click();
    cy.location('pathname').should('eq', '/matches');
    cy.go('back');

    cy.get('#footerOrders').click();
    cy.location('pathname').should('eq', '/orders');
    cy.go('back');
  });
});
