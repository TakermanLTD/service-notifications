/// <reference types="cypress" />

describe('scenarios', () => {
  it("navigation", () => {
    cy.visit('/logout');

    cy.get('#headerAccount').click();
    cy.get('#headerRegister').click();
    cy.location('pathname').should('eq', '/register');
    cy.go('back');

    cy.visit('/logout');
    cy.visit('/login');
    cy.get('input[id=email]').type('tanyo@takerman.net');
    cy.get('input[id=password]').type('Hakerman91!');
    cy.get('button[id=btnSubmit]').click();

    cy.get('#headerOrders').click();
    cy.location('pathname').should('eq', '/orders');
    cy.go('back');

    cy.get('#headerMatches').click();
    cy.location('pathname').should('eq', '/matches');
    cy.go('back');

    cy.get('#headerAdmin').click();
    cy.location('pathname').should('eq', '/admin');
    cy.go('back');

    cy.get('#headerAccount').click();
    cy.get('#headerProfile').click();
    cy.location('pathname').should('eq', '/profile');
    cy.go('back');

    cy.get('#headerAccount').click();
    cy.get('#headerGallery').click();
    cy.location('pathname').should('eq', '/user-gallery');
    cy.go('back');

  });

  xit("register", () => {
    let id = + Math.random();
    alert(id);
    cy.visit('/register');
    cy.get('input[id=firstName]').type('Test' + id);
    cy.get('input[id=lastName]').type('Test' + id);
    cy.get('input[id=email]').type('test' + id + '@takerman.net');
    cy.get('#genderMan').check();
    cy.get('input[id=password]').type('Hakerman91!');
    cy.get('input[id=confirmPassword]').type('Hakerman91!');
    cy.get('button[id=btnSubmit]').click();
    cy.url().should('include', '/login');
  });
  it("save date", () => { });
  it("buy date", () => { });
  it("enter date", () => { });
  it("vote", () => { });
  it("chat", () => { });
  xit("full scenario", () => {
    cy.visit('/logout');
    cy.visit('/login');
    cy.get('input[id=email]').type('tanyo@takerman.net');
    cy.get('input[id=password]').type('Hakerman91!');
    cy.get('button[id=btnSubmit]').click();
    cy.url().should('include', '/');
    cy.visit('/admin');
    cy.visit('/logout');

    for (let i = 0; i < 10; i++) {
      // register
      cy.visit('/register');
      cy.get('input[id=firstName]').type('Test' + i);
      cy.get('input[id=lastName]').type('Test' + i);
      cy.get('input[id=email]').type('test' + i + '@takerman.net');
      if (i < 5) {
        cy.get('#genderMan').check();
      } else {
        cy.get('#genderWoman').check();
      }
      cy.get('input[id=password]').type('Hakerman91!');
      cy.get('input[id=confirmPassword]').type('Hakerman91!');
      cy.get('button[id=btnSubmit]').click();
      cy.clock().tick(500);

      cy.visit('/login');
      cy.get('input[id=email]').type('test' + i + '@takerman.net');
      cy.get('input[id=password]').type('Hakerman91!');
      cy.get('button[id=btnSubmit]').click();
      cy.url().should('include', '/');
      cy.clock().tick(500);

      /*
      cy.get('.btn-save-date').each((element) => {
        cy.get(element).click();
      });
      */
    }

    cy.visit('/logout');
    cy.visit('/login');
    cy.get('input[id=email]').type('tanyo@takerman.net');
    cy.get('input[id=password]').type('Hakerman91!');
    cy.get('button[id=btnSubmit]').click();
    cy.url().should('include', '/');
    cy.visit('/admin');
    cy.get('.row-date').each((el) => {
      if (cy.get(el).find('.tbx-menCount').first().val() >= cy.get(el).find('.tbx-maxMen').first().val()) {
        cy.get(el).find('.ddl-status').first().val(2);
        cy.get(el).find('.btn-save').first().click();
      }
    });
    cy.visit('/logout');

    for (let i = 0; i < 10; i++) {
      cy.visit('/login');
      cy.get('input[id=email]').type('test' + i + '@takerman.net');
      cy.get('input[id=password]').type('Hakerman91!');
      cy.get('button[id=btnSubmit]').click();
      cy.url().should('include', '/');

      cy.get('.date-card-image').each((date) => {
        cy.get(date).click();
        cy.clock().tick(1000);
        cy.get('.pay-button').first().click();
      });
    }

    // for each ten customers
    // login
    // visit the link
    // vote
    // check your matches
  });
});
