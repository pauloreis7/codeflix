package com.pauloreis.catalog.admin.domain.category;

import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;

import com.pauloreis.catalog.admin.domain.exceptions.DomainException;
import com.pauloreis.catalog.admin.domain.validation.handler.ThrowsValidationHandler;

public class CategoryTest {

  @Test
  public void givenAValidParams_whenCallNewCategory_thenInstantiateACategory() {
    final var expectedName = "name";
    final var expectedDescription = "Some description";
    final var expectedIsActive = true;

    final var actualCategory = Category.newCategory(expectedName, expectedDescription, expectedIsActive);

    Assertions.assertNotNull(actualCategory);
    Assertions.assertNotNull(actualCategory.getId());
    Assertions.assertEquals(expectedName, actualCategory.getName());
    Assertions.assertEquals(expectedDescription, actualCategory.getDescription());
    Assertions.assertEquals(expectedIsActive, actualCategory.isActive());
    Assertions.assertNotNull(actualCategory.getCreatedAt());
    Assertions.assertNotNull(actualCategory.getUpdatedAt());
    Assertions.assertNull(actualCategory.getDeletedAt());
  }

  @Test
  public void givenAnInvalidNullName_whenCallNewCategoryAndValidate_thenShouldReceiveError() {
    final String expectedName = null;
    final var expectedErrorCount = 1;
    final var expectedErrorMessage = "'name' should not be null";
    final var expectedDescription = "Some description";
    final var expectedIsActive = true;

    final var actualCategory = Category.newCategory(expectedName, expectedDescription, expectedIsActive);

    final var actualException = Assertions.assertThrows(DomainException.class,
        () -> actualCategory.validate(new ThrowsValidationHandler()));

    Assertions.assertEquals(expectedErrorCount, actualException.getErrors().size());
    Assertions.assertEquals(expectedErrorMessage, actualException.getErrors().get(0).message());
  }

  @Test
  public void givenAnInvalidEmptyName_whenCallNewCategoryAndValidate_thenShouldReceiveError() {
    final var expectedName = "  ";
    final var expectedErrorCount = 1;
    final var expectedErrorMessage = "'name' should not be empty";
    final var expectedDescription = "Some description";
    final var expectedIsActive = true;

    final var actualCategory = Category.newCategory(expectedName, expectedDescription, expectedIsActive);

    final var actualException = Assertions.assertThrows(DomainException.class,
        () -> actualCategory.validate(new ThrowsValidationHandler()));

    Assertions.assertEquals(expectedErrorCount, actualException.getErrors().size());
    Assertions.assertEquals(expectedErrorMessage, actualException.getErrors().get(0).message());
  }

  @Test
  public void givenAnInvalidNameLengthLessThan3_whenCallNewCategoryAndValidate_thenShouldReceiveError() {
    final var expectedName = "Fi ";
    final var expectedErrorCount = 1;
    final var expectedErrorMessage = "'name' must be between 3 and 255 characters";
    final var expectedDescription = "Some description";
    final var expectedIsActive = true;

    final var actualCategory = Category.newCategory(expectedName, expectedDescription, expectedIsActive);

    final var actualException = Assertions.assertThrows(DomainException.class,
        () -> actualCategory.validate(new ThrowsValidationHandler()));

    Assertions.assertEquals(expectedErrorCount, actualException.getErrors().size());
    Assertions.assertEquals(expectedErrorMessage, actualException.getErrors().get(0).message());
  }

  @Test
  public void givenAnInvalidNameLengthMoreThan255_whenCallNewCategoryAndValidate_thenShouldReceiveError() {
    final var expectedName = """
          Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed ultricies pulvinar nulla, interdum blandit ipsum. Integer nec justo blandit, dictum sapien quis, lacinia justo. Vivamus hendrerit nunc et diam luctus scelerisque. Pellentesque non facilisis felis.
        """;

    final var expectedErrorCount = 1;
    final var expectedErrorMessage = "'name' must be between 3 and 255 characters";
    final var expectedDescription = "Some description";
    final var expectedIsActive = true;

    final var actualCategory = Category.newCategory(expectedName, expectedDescription, expectedIsActive);

    final var actualException = Assertions.assertThrows(DomainException.class,
        () -> actualCategory.validate(new ThrowsValidationHandler()));

    Assertions.assertEquals(expectedErrorCount, actualException.getErrors().size());
    Assertions.assertEquals(expectedErrorMessage, actualException.getErrors().get(0).message());
  }

  @Test
  public void givenAValidEmptyDescription_whenCallNewCategoryAndValidate_thenShouldReceiveOK() {
    final var expectedName = "Movies";
    final var expectedDescription = "  ";
    final var expectedIsActive = true;

    final var actualCategory = Category.newCategory(expectedName, expectedDescription, expectedIsActive);

    Assertions.assertDoesNotThrow(() -> actualCategory.validate(new ThrowsValidationHandler()));

    Assertions.assertNotNull(actualCategory);
    Assertions.assertNotNull(actualCategory.getId());
    Assertions.assertEquals(expectedName, actualCategory.getName());
    Assertions.assertEquals(expectedDescription, actualCategory.getDescription());
    Assertions.assertEquals(expectedIsActive, actualCategory.isActive());
    Assertions.assertNotNull(actualCategory.getCreatedAt());
    Assertions.assertNotNull(actualCategory.getUpdatedAt());
    Assertions.assertNull(actualCategory.getDeletedAt());
  }

  @Test
  public void givenAValidFalseIsActive_whenCallNewCategoryAndValidate_thenShouldReceiveOK() {
    final var expectedName = "Movies";
    final var expectedDescription = "Some description";
    final var expectedIsActive = false;

    final var actualCategory = Category.newCategory(expectedName, expectedDescription, expectedIsActive);

    Assertions.assertDoesNotThrow(() -> actualCategory.validate(new ThrowsValidationHandler()));

    Assertions.assertNotNull(actualCategory);
    Assertions.assertNotNull(actualCategory.getId());
    Assertions.assertEquals(expectedName, actualCategory.getName());
    Assertions.assertEquals(expectedDescription, actualCategory.getDescription());
    Assertions.assertEquals(expectedIsActive, actualCategory.isActive());
    Assertions.assertNotNull(actualCategory.getCreatedAt());
    Assertions.assertNotNull(actualCategory.getUpdatedAt());
    Assertions.assertNotNull(actualCategory.getDeletedAt());
  }
}
