package com.pauloreis.catalog.admin.application.category.retrieve.get;

import static org.mockito.ArgumentMatchers.eq;
import static org.mockito.Mockito.when;

import java.util.Optional;

import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.jupiter.MockitoExtension;

import com.pauloreis.catalog.admin.domain.category.Category;
import com.pauloreis.catalog.admin.domain.category.CategoryGateway;
import com.pauloreis.catalog.admin.domain.category.CategoryID;
import com.pauloreis.catalog.admin.domain.exceptions.DomainException;

@ExtendWith(MockitoExtension.class)
public class GetCategoryByIdUseCaseTest {
  @InjectMocks
  private DefaultGetCategoryByIdUseCase useCase;

  @Mock
  private CategoryGateway categoryGateway;

  @BeforeEach
  void cleanUp() {
    Mockito.reset(categoryGateway);
  }

  @Test
  public void givenAValidId_whenCallsGetCategory_shouldReturnCategory() {
    final var expectedName = "Movies";
    final var expectedDescription = "Some description";
    final var expectedIsActive = true;

    final var aCategory = Category.newCategory(expectedName, expectedDescription, expectedIsActive);

    final var expectedId = aCategory.getId();

    when(categoryGateway.findById(eq(expectedId)))
        .thenReturn(Optional.of(aCategory.clone()));

    final var actualCategory = useCase.execute(expectedId.getValue());

    Assertions.assertEquals(expectedId, actualCategory.id());
    Assertions.assertEquals(expectedName, actualCategory.name());
    Assertions.assertEquals(expectedDescription, actualCategory.description());
    Assertions.assertEquals(expectedIsActive, actualCategory.isActive());
    Assertions.assertEquals(aCategory.getCreatedAt(), actualCategory.createdAt());
    Assertions.assertEquals(aCategory.getUpdatedAt(), actualCategory.updatedAt());
    Assertions.assertEquals(aCategory.getDeletedAt(), actualCategory.deletedAt());
  }

  @Test
  public void givenAInvalidId_whenCallsGetCategory_shouldReturnNotFound() {
    final var expectedId = CategoryID.from("123");

    final var expectedErrorCount = 1;
    final var expectedErrorMessage = "Category with ID 123 was not found";

    when(categoryGateway.findById(eq(expectedId)))
        .thenReturn(Optional.empty());

    final var actualException = Assertions.assertThrows(
        DomainException.class,
        () -> useCase.execute(expectedId.getValue()));

    Assertions.assertEquals(expectedErrorCount, actualException.getErrors().size());
    Assertions.assertEquals(expectedErrorMessage, actualException.getMessage());
  }

  @Test
  public void givenAValidId_whenGatewayThrowsException_shouldReturnException() {
    final var expectedId = CategoryID.from("123");

    final var expectedErrorMessage = "Gateway error";

    when(categoryGateway.findById(eq(expectedId)))
        .thenThrow(new IllegalStateException(expectedErrorMessage));

    final var actualException = Assertions.assertThrows(
        IllegalStateException.class,
        () -> useCase.execute(expectedId.getValue()));

    Assertions.assertEquals(expectedErrorMessage, actualException.getMessage());
  }

}
