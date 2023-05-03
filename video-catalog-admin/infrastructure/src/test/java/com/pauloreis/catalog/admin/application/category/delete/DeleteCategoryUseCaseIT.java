package com.pauloreis.catalog.admin.application.category.delete;

import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;
import org.mockito.Mockito;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.mock.mockito.SpyBean;

import com.pauloreis.catalog.admin.IntegrationTest;
import com.pauloreis.catalog.admin.domain.category.Category;
import com.pauloreis.catalog.admin.domain.category.CategoryGateway;
import com.pauloreis.catalog.admin.domain.category.CategoryID;
import com.pauloreis.catalog.admin.infrastructure.category.persistence.CategoryJpaEntity;
import com.pauloreis.catalog.admin.infrastructure.category.persistence.CategoryRepository;

import java.util.Arrays;

import static org.mockito.ArgumentMatchers.eq;
import static org.mockito.Mockito.doThrow;
import static org.mockito.Mockito.times;

@IntegrationTest
public class DeleteCategoryUseCaseIT {

  @Autowired
  private DeleteCategoryUseCase useCase;

  @Autowired
  private CategoryRepository categoryRepository;

  @SpyBean
  private CategoryGateway categoryGateway;

  @Test
  public void givenAValidId_whenCallsDeleteCategory_shouldBeOK() {
    final var aCategory = Category.newCategory("Movies", "Some description", true);
    final var expectedId = aCategory.getId();

    save(aCategory);

    Assertions.assertEquals(1, categoryRepository.count());

    Assertions.assertDoesNotThrow(() -> useCase.execute(expectedId.getValue()));

    Assertions.assertEquals(0, categoryRepository.count());
  }

  @Test
  public void givenAInvalidId_whenCallsDeleteCategory_shouldBeOK() {
    final var expectedId = CategoryID.from("123");

    Assertions.assertEquals(0, categoryRepository.count());

    Assertions.assertDoesNotThrow(() -> useCase.execute(expectedId.getValue()));

    Assertions.assertEquals(0, categoryRepository.count());
  }

  @Test
  public void givenAValidId_whenGatewayThrowsException_shouldReturnException() {
    final var aCategory = Category.newCategory("Movies", "Some description", true);
    final var expectedId = aCategory.getId();

    doThrow(new IllegalStateException("Gateway error"))
        .when(categoryGateway).deleteById(eq(expectedId));

    Assertions.assertThrows(IllegalStateException.class, () -> useCase.execute(expectedId.getValue()));

    Mockito.verify(categoryGateway, times(1)).deleteById(eq(expectedId));
  }

  private void save(final Category... aCategory) {
    categoryRepository.saveAllAndFlush(
        Arrays.stream(aCategory)
            .map(CategoryJpaEntity::from)
            .toList());
  }
}